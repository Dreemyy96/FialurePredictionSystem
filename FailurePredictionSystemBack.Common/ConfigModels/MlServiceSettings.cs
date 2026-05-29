namespace FailurePredictionSystemBack.Common.ConfigModels;

public class MlServiceSettings
{
    public string BaseUrl { get; set; }
    public int ProcessingIntervalSeconds { get; set; }
    public int BatchSize { get; set; }
}