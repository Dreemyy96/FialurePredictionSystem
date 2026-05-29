using System;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class Prediction
{
    public Guid Id { get; init; }
    public Guid EquipmentId { get; init; }
    public virtual Equipment Equipment { get; init; }
    public Guid MetricId { get; init; }
    public virtual Metric Metric { get; init; }
    public EquipmentState PredictedState { get; init; }
    public double NormalProbability { get; init; }
    public double WarningProbability { get; init; }
    public double CriticalProbability { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public virtual Alert Alert { get; private set; }

    protected Prediction()
    {
    }

    public Prediction(
        Guid equipmentId,
        Guid metricId,
        EquipmentState predictedState,
        double normalProbability,
        double warningProbability,
        double criticalProbability)
    {
        Id = Guid.NewGuid();
        EquipmentId = equipmentId;
        MetricId = metricId;
        PredictedState = predictedState;
        NormalProbability = normalProbability;
        WarningProbability = warningProbability;
        CriticalProbability = criticalProbability;
        CreatedAtUtc = DateTime.UtcNow;
    }
}