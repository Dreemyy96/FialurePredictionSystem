using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class MetricDashboardResponse
{
    public Guid Id { get; }
    public DateTime TimestampUtc { get; }
    public double CpuUsagePercent { get; }
    public double RamUsagePercent { get; }
    public double DiskUsagePercent { get; }
    public double FreeDiskSpaceGb { get; }
    public double TemperatureCelsius { get; }
    public int ErrorCount { get; }
    public double UptimeHours { get; }
    public EquipmentState? State { get; }
    public PredictionStatus PredictionStatus { get; }

    private MetricDashboardResponse(Metric metric)
    {
        Id = metric.Id;
        TimestampUtc = metric.TimestampUtc;
        CpuUsagePercent = metric.CpuUsagePercent;
        RamUsagePercent = metric.RamUsagePercent;
        DiskUsagePercent = metric.DiskUsagePercent;
        FreeDiskSpaceGb = metric.FreeDiskSpaceGb;
        TemperatureCelsius = metric.TemperatureCelsius;
        ErrorCount = metric.ErrorCount;
        State = metric.State;
        PredictionStatus = metric.PredictionStatus;
        UptimeHours = metric.UptimeHours;
    }

    public static MetricDashboardResponse Create(Metric metric)
    {
        return new MetricDashboardResponse(metric);
    }
}