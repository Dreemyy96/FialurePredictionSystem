using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class EquipmentDashboardResponse
{
    public Guid EquipmentId { get; }
    public string EquipmentName { get; }
    public string Hostname { get; }
    public EquipmentType EquipmentType { get; }
    public string Location { get; }
    public MetricDashboardResponse LatestMetric { get; }
    public PredictionDashboardResponse LatestPrediction { get; }
    public int MetricsCount { get; }
    public int PredictionsCount { get; }
    public int NormalPredictionsCount { get; }
    public int WarningPredictionsCount { get; }
    public int CriticalPredictionsCount { get; }
    public int PendingMetricsCount { get; }
    public int ProcessedMetricsCount { get; }
    public int FailedMetricsCount { get; }

    private EquipmentDashboardResponse(Equipment equipment, MetricDashboardResponse latestMetric,
        PredictionDashboardResponse prediction, int metricsCount, int predictionsCount, int normalCount,
        int warningCount, int criticalCount, int pendingCount, int processedCount, int failedCount)
    {
        EquipmentId = equipment.Id;
        EquipmentName = equipment.Name;
        Hostname = equipment.Hostname;
        EquipmentType = equipment.Type;
        Location = equipment.Location;
        LatestMetric = latestMetric;
        LatestPrediction = prediction;
        MetricsCount = metricsCount;
        PredictionsCount = predictionsCount;
        NormalPredictionsCount = normalCount;
        WarningPredictionsCount = warningCount;
        CriticalPredictionsCount = criticalCount;
        PendingMetricsCount = pendingCount;
        ProcessedMetricsCount = processedCount;
        FailedMetricsCount = failedCount;
    }

    public static EquipmentDashboardResponse Create(Equipment equipment, MetricDashboardResponse latestMetric,
        PredictionDashboardResponse prediction, int metricsCount, int predictionsCount, int normalCount,
        int warningCount, int criticalCount, int pendingCount, int processedCount, int failedCount)
    {
        return new EquipmentDashboardResponse(equipment, latestMetric, prediction, metricsCount, predictionsCount,
            normalCount, warningCount, criticalCount, pendingCount, processedCount, failedCount);
    }
}