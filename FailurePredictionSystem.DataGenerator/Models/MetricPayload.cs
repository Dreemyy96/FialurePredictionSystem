using System;
using FailurePredictionSystem.DataGenerator.Enums;

namespace FailurePredictionSystem.DataGenerator.Models;

public class MetricPayload
{
    public Guid AgentId { get; set; }
    public Guid EquipmentId { get; set; }
    public string Hostname { get; set; }
    public DateTime TimestampUtc { get; set; }
    public double CpuUsagePercent { get; set; }
    public double RamUsagePercent { get; set; }
    public double DiskUsagePercent { get; set; }
    public double FreeDiskSpaceGb { get; set; }
    public double TemperatureCelsius { get; set; }
    public int ErrorCount { get; set; }
    public EquipmentState? State { get; set; }
    public double UptimeHours { get; set; }
}