using System;

namespace FailurePredictionSystemBack.Core.Models;

public class EquipmentNotificationSubscription
{
    public Guid Id { get; init; }
    public Guid EquipmentId { get; init; }
    public Guid UserId { get; init; }
    public bool IsInAppEnabled { get; private set; }
    public bool IsEmailEnabled { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public virtual Equipment Equipment { get; private set; }
    public virtual User User { get; private set; }

    protected EquipmentNotificationSubscription()
    {
    }

    public EquipmentNotificationSubscription(Guid equipmentId, Guid userId, bool isInAppEnabled, bool isEmailEnabled)
    {
        Id = Guid.NewGuid();
        EquipmentId = equipmentId;
        UserId = userId;
        IsInAppEnabled = isInAppEnabled;
        IsEmailEnabled = isEmailEnabled;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Update(
        bool isInAppEnabled,
        bool isEmailEnabled)
    {
        IsInAppEnabled = isInAppEnabled;
        IsEmailEnabled = isEmailEnabled;
    }
}