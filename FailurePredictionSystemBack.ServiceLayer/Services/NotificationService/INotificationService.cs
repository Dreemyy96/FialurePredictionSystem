using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;

public interface INotificationService
{
    Task CreateForAlertAsync(
        Alert alert,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<NotificationResponse>> GetCurrentUserNotificationsAsync(
        bool? isRead,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<NotificationResponse>> GetAllAsync(
        bool? isRead,
        CancellationToken cancellationToken);

    Task<bool> MarkAsReadAsync(
        Guid notificationId,
        CancellationToken cancellationToken);
}