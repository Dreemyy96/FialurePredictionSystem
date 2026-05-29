using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.EquipmentNotificationSubscriptionService;

public interface IEquipmentNotificationSubscriptionService
{
    Task<EquipmentNotificationSubscriptionResponse> SubscribeAsync(
        Guid equipmentId,
        UpdateEquipmentNotificationSubscriptionRequest request,
        CancellationToken cancellationToken);

    Task<bool> UnsubscribeAsync(
        Guid equipmentId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<EquipmentNotificationSubscriptionResponse>> GetMySubscriptionsAsync(
        CancellationToken cancellationToken);
}