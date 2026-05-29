namespace FailurePredictionSystemBack.Common.Models;

public class MlPredictionProbabilities
{
    public double Normal { get; init; }

    public double Warning { get; init; }

    public double Critical { get; init; }
}