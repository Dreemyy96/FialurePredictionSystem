using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class PredictionDashboardResponse
{
    public Guid Id { get; }
    public Guid MetricId { get; }
    public EquipmentState PredictedState { get; }
    public int PredictedStateCode => (int)PredictedState;
    public string PredictedStateName => PredictedState.ToString();
    public double NormalProbability { get; }
    public double WarningProbability { get; }
    public double CriticalProbability { get; }
    public DateTime CreatedAtUtc { get; }

    private PredictionDashboardResponse(Prediction prediction)
    {
        Id = prediction.Id;
        MetricId = prediction.MetricId;
        PredictedState = prediction.PredictedState;
        NormalProbability = prediction.NormalProbability;
        WarningProbability = prediction.WarningProbability;
        CriticalProbability = prediction.CriticalProbability;
        CreatedAtUtc = prediction.CreatedAtUtc;
    }

    public static PredictionDashboardResponse Create(Prediction prediction)
    {
        return new PredictionDashboardResponse(prediction);
    }
}