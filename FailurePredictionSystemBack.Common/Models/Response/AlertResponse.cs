using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class AlertResponse
{
    public Guid Id { get; }
    public Guid EquipmentId { get; }
    public Guid PredictionId { get; }
    public AlertSeverity Severity { get; }
    public int SeverityCode => (int)Severity;
    public string SeverityName => Severity.ToString();
    public string Title { get; }
    public string Message { get; }
    public bool IsResolved { get; }
    public DateTime CreatedAtUtc { get; }
    public DateTime? ResolvedAtUtc { get; }

    private AlertResponse(Alert alert)
    {
        Id = alert.Id;
        EquipmentId = alert.EquipmentId;
        PredictionId = alert.PredictionId;
        Severity = alert.Severity;
        Title = alert.Title;
        Message = alert.Message;
        IsResolved = alert.IsResolved;
        CreatedAtUtc = alert.CreatedAtUtc;
        ResolvedAtUtc = alert.ResolvedAtUtc;
    }

    public static AlertResponse Create(Alert alert)
    {
        return new AlertResponse(alert);
    }
}