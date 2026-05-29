using System;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Common.Models.Request;

public class CreateMetricRequest
{
    public Guid AgentId { get; init; }
    public Guid EquipmentId { get; init; }
    public string Hostname { get; init; }
    public DateTime TimestampUtc { get; init; }
    public double CpuUsagePercent { get; init; }
    public double RamUsagePercent { get; init; }
    public double DiskUsagePercent { get; init; }
    public double FreeDiskSpaceGb { get; init; }
    public double TemperatureCelsius { get; init; }
    public int ErrorCount { get; init; }
    public EquipmentState? State { get; init; }
    public double UptimeHours { get; init; }
}