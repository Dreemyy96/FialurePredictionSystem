using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly FailureSystemDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEmailNotificationSender _emailSender;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        FailureSystemDbContext dbContext,
        ICurrentUserService currentUserService,
        IEmailNotificationSender emailSender,
        ILogger<NotificationService> logger)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task CreateForAlertAsync(
        Alert alert,
        CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == alert.EquipmentId,
                cancellationToken);

        if (equipment is null)
        {
            _logger.LogWarning(
                "Equipment not found for alert. AlertId: {AlertId}, EquipmentId: {EquipmentId}",
                alert.Id,
                alert.EquipmentId);

            return;
        }

        var subscriptions = await _dbContext.EquipmentNotificationSubscriptions
            .AsNoTracking()
            .Include(x => x.User)
            .Where(x => x.EquipmentId == alert.EquipmentId &&
                        x.User.IsActive)
            .ToListAsync(cancellationToken);

        var recipients = new Dictionary<Guid, NotificationRecipient>();


        var owner = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == equipment.CreatedByUserId &&
                     x.IsActive,
                cancellationToken);

        if (owner is not null)
        {
            recipients[owner.Id] = new NotificationRecipient
            {
                UserId = owner.Id,
                Email = owner.Email,
                IsInAppEnabled = true,
                IsEmailEnabled = true
            };
        }


        foreach (var subscription in subscriptions)
        {
            if (recipients.TryGetValue(subscription.UserId, out var existing))
            {
                existing.IsInAppEnabled = existing.IsInAppEnabled || subscription.IsInAppEnabled;
                existing.IsEmailEnabled = existing.IsEmailEnabled || subscription.IsEmailEnabled;
            }
            else
            {
                recipients[subscription.UserId] = new NotificationRecipient
                {
                    UserId = subscription.UserId,
                    Email = subscription.User.Email,
                    IsInAppEnabled = subscription.IsInAppEnabled,
                    IsEmailEnabled = subscription.IsEmailEnabled
                };
            }
        }

        if (recipients.Count == 0)
        {
            _logger.LogWarning(
                "No notification recipients found. AlertId: {AlertId}, EquipmentId: {EquipmentId}",
                alert.Id,
                alert.EquipmentId);

            return;
        }

        foreach (var recipient in recipients.Values)
        {
            if (recipient.IsInAppEnabled)
            {
                var inAppNotification = new Notification(
                    recipient.UserId,
                    alert.Id,
                    NotificationChannel.InApp,
                    alert.Title,
                    alert.Message,
                    null);

                inAppNotification.MarkAsSent();

                await _dbContext.Notifications.AddAsync(
                    inAppNotification,
                    cancellationToken);
            }

            if (recipient.IsEmailEnabled)
            {
                var emailNotification = new Notification(
                    recipient.UserId,
                    alert.Id,
                    NotificationChannel.Email,
                    alert.Title,
                    alert.Message,
                    null);

                await _dbContext.Notifications.AddAsync(
                    emailNotification,
                    cancellationToken);

                try
                {
                    await _emailSender.SendAsync(
                        emailNotification,
                        recipient.Email,
                        cancellationToken);

                    emailNotification.MarkAsSent();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to send email notification. UserId: {UserId}, AlertId: {AlertId}",
                        recipient.UserId,
                        alert.Id);

                    emailNotification.MarkAsFailed(ex.Message);
                }
            }
        }
    }

    public async Task<IReadOnlyList<NotificationResponse>> GetCurrentUserNotificationsAsync(
        bool? isRead,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            throw new InvalidOperationException("Пользователь не авторизован.");

        var query = _dbContext.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId.Value);

        if (isRead.HasValue)
        {
            query = query.Where(x => x.IsRead == isRead.Value);
        }

        return await query
            .OrderBy(x => x.IsRead)
            .ThenByDescending(x => x.CreatedAtUtc)
            .Select(x => NotificationResponse.Create(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<NotificationResponse>> GetAllAsync(
        bool? isRead,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .AsQueryable();

        if (isRead.HasValue)
        {
            query = query.Where(x => x.IsRead == isRead.Value);
        }

        return await query
            .OrderBy(x => x.IsRead)
            .ThenByDescending(x => x.CreatedAtUtc)
            .Select(x => NotificationResponse.Create(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> MarkAsReadAsync(
        Guid notificationId,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            throw new InvalidOperationException("Пользователь не авторизован.");

        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(
                x => x.Id == notificationId &&
                     x.UserId == userId.Value,
                cancellationToken);

        if (notification is null)
            return false;

        notification.MarkAsRead();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private sealed class NotificationRecipient
    {
        public Guid UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public bool IsInAppEnabled { get; set; }

        public bool IsEmailEnabled { get; set; }
    }
}