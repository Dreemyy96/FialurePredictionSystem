using System;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class EquipmentNotificationSubscriptionResponse
{
    public Guid Id { get; }
    public Guid EquipmentId { get; }
    public Guid UserId { get; }
    public bool IsInAppEnabled { get; }
    public bool IsEmailEnabled { get; }
    public DateTime CreatedAtUtc { get; }

    private EquipmentNotificationSubscriptionResponse(
        EquipmentNotificationSubscription equipmentNotificationSubscription)
    {
        Id = equipmentNotificationSubscription.Id;
        EquipmentId = equipmentNotificationSubscription.EquipmentId;
        UserId = equipmentNotificationSubscription.UserId;
        IsInAppEnabled = equipmentNotificationSubscription.IsInAppEnabled;
        IsEmailEnabled = equipmentNotificationSubscription.IsEmailEnabled;
        CreatedAtUtc = equipmentNotificationSubscription.CreatedAtUtc;
    }

    public static EquipmentNotificationSubscriptionResponse Create(
        EquipmentNotificationSubscription equipmentNotificationSubscription)
    {
        return new EquipmentNotificationSubscriptionResponse(equipmentNotificationSubscription);
    }
}