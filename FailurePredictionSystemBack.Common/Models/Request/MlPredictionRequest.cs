namespace FailurePredictionSystemBack.Common.Models.Request;

public class MlPredictionRequest
{
    public double CpuUsagePercent { get; init; }
    public double RamUsagePercent { get; init; }
    public double DiskUsagePercent { get; init; }
    public double FreeDiskSpaceGb { get; init; }
    public double TemperatureCelsius { get; init; }
    public int ErrorCount { get; init; }
    public double UptimeHours { get; init; }
}