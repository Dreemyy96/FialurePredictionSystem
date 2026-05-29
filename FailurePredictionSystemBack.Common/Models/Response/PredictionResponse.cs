using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class PredictionResponse
{
    public Guid Id { get; }
    public Guid EquipmentId { get; }
    public Guid MetricId { get; }
    public EquipmentState PredictedState { get; }
    public int PredictedStateCode => (int)PredictedState;
    public double NormalProbability { get; }
    public double WarningProbability { get; }
    public double CriticalProbability { get; }
    public DateTime CreatedAtUtc { get; }

    private PredictionResponse(Prediction prediction)
    {
        Id = prediction.Id;
        EquipmentId = prediction.EquipmentId;
        MetricId = prediction.MetricId;
        PredictedState = prediction.PredictedState;
        NormalProbability = prediction.NormalProbability;
        WarningProbability = prediction.WarningProbability;
        CriticalProbability = prediction.CriticalProbability;
        CreatedAtUtc = prediction.CreatedAtUtc;
    }

    public static PredictionResponse Create(Prediction prediction)
    {
        return new PredictionResponse(prediction);
    }
}