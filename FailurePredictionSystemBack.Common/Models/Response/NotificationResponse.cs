using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class NotificationResponse
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid AlertId { get; }
    public NotificationChannel Channel { get; }
    public int ChannelCode => (int)Channel;
    public string ChannelName => Channel.ToString();
    public NotificationStatus Status { get; }
    public int StatusCode => (int)Status;
    public string StatusName => Status.ToString();
    public string Subject { get; }
    public string Message { get; }
    public bool IsRead { get; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? SentAtUtc { get; }
    public string? ErrorMessage { get; }

    private NotificationResponse(Notification notification)
    {
        Id = notification.Id;
        UserId = notification.UserId;
        AlertId = notification.AlertId;
        Channel = notification.Channel;
        Status = notification.Status;
        Subject = notification.Subject;
        Message = notification.Message;
        IsRead = notification.IsRead;
        CreatedAtUtc = notification.CreatedAtUtc;
        SentAtUtc = notification.SentAtUtc;
        ErrorMessage = notification.ErrorMessage;
    }

    public static NotificationResponse Create(Notification notification)
    {
        return new NotificationResponse(notification);
    }
}