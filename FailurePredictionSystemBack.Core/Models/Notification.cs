using System;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class Notification
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid AlertId { get; init; }
    public NotificationChannel Channel { get; private set; }
    public NotificationStatus Status { get; private set; }
    public string Subject { get; private set; }
    public string Message { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? SentAtUtc { get; private set; }
    public string ErrorMessage { get; private set; }
    public virtual User User { get; private set; }
    public virtual Alert Alert { get; private set; }

    protected Notification()
    {
    }

    public Notification(Guid userId, Guid alertId, NotificationChannel channel,
        string subject, string message, string errorMessage)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        AlertId = alertId;
        Channel = channel;
        Status = NotificationStatus.Pending;
        Subject = subject;
        Message = message;
        IsRead = false;
        CreatedAtUtc = DateTime.UtcNow;
        SentAtUtc = null;
        ErrorMessage = errorMessage;
    }

    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
        SentAtUtc = DateTime.UtcNow;
        ErrorMessage = null;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
    }

    public void MarkAsRead()
    {
        if (IsRead)
            return;

        IsRead = true;
    }
}