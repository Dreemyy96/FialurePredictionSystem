using System;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class Metric
{
    public Guid Id { get; init; }
    public Guid EquipmentId { get; init; }
    public Guid AgentId { get; init; }
    public string Hostname { get; init; }
    public DateTime TimestampUtc { get; init; }
    public double CpuUsagePercent { get; init; }
    public double RamUsagePercent { get; init; }
    public double DiskUsagePercent { get; init; }
    public double FreeDiskSpaceGb { get; init; }
    public double TemperatureCelsius { get; init; }
    public int ErrorCount { get; init; }
    public double UptimeHours { get; init; }
    public EquipmentState? State { get; init; }
    public PredictionStatus PredictionStatus { get; private set; }
    public DateTime ReceivedAtUtc { get; init; }

    public virtual Prediction Prediction { get; private set; }

    public virtual Equipment Equipment { get; init; }

    protected Metric()
    {
    }

    public Metric(Guid equipmentId, Guid agentId, string hostname, DateTime timestampUtc, double cpuUsagePercent,
        double ramUsagePercent,
        double diskUsagePercent, double freeDiskSpaceGb, double temperatureCelsius, int errorCount,
        EquipmentState? state, double uptimeHours)

    {
        Id = Guid.NewGuid();
        EquipmentId = equipmentId;
        AgentId = agentId;
        Hostname = hostname;
        TimestampUtc = timestampUtc;
        CpuUsagePercent = cpuUsagePercent;
        RamUsagePercent = ramUsagePercent;
        DiskUsagePercent = diskUsagePercent;
        FreeDiskSpaceGb = freeDiskSpaceGb;
        TemperatureCelsius = temperatureCelsius;
        ErrorCount = errorCount;
        UptimeHours = uptimeHours;
        ReceivedAtUtc = DateTime.UtcNow;
        State = state;
        PredictionStatus = PredictionStatus.Pending;
    }

    public void MarkPredictionProcessed()
    {
        PredictionStatus = PredictionStatus.Processed;
    }

    public void MarkPredictionFailed()
    {
        PredictionStatus = PredictionStatus.Failed;
    }

    public void MarkPredictionPending()
    {
        PredictionStatus = PredictionStatus.Pending;
    }
}