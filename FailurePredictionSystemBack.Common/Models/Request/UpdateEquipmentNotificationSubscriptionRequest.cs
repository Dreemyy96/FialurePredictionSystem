namespace FailurePredictionSystemBack.Common.Models.Request;

public class UpdateEquipmentNotificationSubscriptionRequest
{
    public bool IsInAppEnabled { get; set; } = true;
    public bool IsEmailEnabled { get; set; } = true;
}