namespace FailurePredictionSystemBack.Common.Models.Response;

public class MlPredictionResponse
{
    public int PredictedStateCode { get; }
    public string PredictedState { get; }
    public MlPredictionProbabilities Probabilities { get; }

    public MlPredictionResponse(int predictedStateCode, string predictedState, MlPredictionProbabilities probabilities)
    {
        PredictedStateCode = predictedStateCode;
        PredictedState = predictedState;
        Probabilities = probabilities;
    }
}