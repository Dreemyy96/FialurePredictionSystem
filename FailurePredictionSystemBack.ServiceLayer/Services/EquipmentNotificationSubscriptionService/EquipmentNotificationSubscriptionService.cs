using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.EquipmentNotificationSubscriptionService;

public class EquipmentNotificationSubscriptionService : IEquipmentNotificationSubscriptionService
{
    private readonly FailureSystemDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public EquipmentNotificationSubscriptionService(
        FailureSystemDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<EquipmentNotificationSubscriptionResponse> SubscribeAsync(
        Guid equipmentId,
        UpdateEquipmentNotificationSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            throw new InvalidOperationException("Пользователь не авторизован.");

        var equipmentExists = await _dbContext.Equipments
            .AnyAsync(x => x.Id == equipmentId, cancellationToken);

        if (!equipmentExists)
            throw new InvalidOperationException("Оборудование не найдено.");

        var subscription = await _dbContext.EquipmentNotificationSubscriptions
            .FirstOrDefaultAsync(
                x => x.EquipmentId == equipmentId &&
                     x.UserId == userId.Value,
                cancellationToken);

        if (subscription is null)
        {
            subscription = new EquipmentNotificationSubscription(
                equipmentId: equipmentId,
                userId: userId.Value,
                isInAppEnabled: request.IsInAppEnabled,
                isEmailEnabled: request.IsEmailEnabled);

            await _dbContext.EquipmentNotificationSubscriptions.AddAsync(
                subscription,
                cancellationToken);
        }
        else
        {
            subscription.Update(
                request.IsInAppEnabled,
                request.IsEmailEnabled);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return EquipmentNotificationSubscriptionResponse.Create(subscription);
    }

    public async Task<bool> UnsubscribeAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            throw new InvalidOperationException("Пользователь не авторизован.");

        var subscription = await _dbContext.EquipmentNotificationSubscriptions
            .FirstOrDefaultAsync(
                x => x.EquipmentId == equipmentId &&
                     x.UserId == userId.Value,
                cancellationToken);

        if (subscription is null)
            return false;

        _dbContext.EquipmentNotificationSubscriptions.Remove(subscription);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IReadOnlyList<EquipmentNotificationSubscriptionResponse>> GetMySubscriptionsAsync(
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (userId is null)
            throw new InvalidOperationException("Пользователь не авторизован.");

        return await _dbContext.EquipmentNotificationSubscriptions
            .AsNoTracking()
            .Where(x => x.UserId == userId.Value)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => EquipmentNotificationSubscriptionResponse.Create(x))
            .ToListAsync(cancellationToken);
    }
}