using System;
using System.Collections.Generic;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class Alert
{
    public Guid Id { get; init; }
    public Guid EquipmentId { get; init; }
    public Guid PredictionId { get; init; }
    public AlertSeverity Severity { get; init; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public bool IsResolved { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? ResolvedAtUtc { get; private set; }
    public virtual Equipment Equipment { get; private set; }
    public virtual Prediction Prediction { get; private set; }

    public virtual ICollection<Notification> Notifications { get; private set; }

    protected Alert()
    {
    }

    public Alert(
        Guid equipmentId,
        Guid predictionId,
        AlertSeverity severity,
        string title,
        string message)
    {
        Id = Guid.NewGuid();
        EquipmentId = equipmentId;
        PredictionId = predictionId;
        Severity = severity;
        Title = title;
        Message = message;
        IsResolved = false;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Resolve()
    {
        if (IsResolved)
            return;

        IsResolved = true;
        ResolvedAtUtc = DateTime.UtcNow;
    }
}