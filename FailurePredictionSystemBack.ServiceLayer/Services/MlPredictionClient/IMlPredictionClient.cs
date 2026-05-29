using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.MlPredictionClient;

public interface IMlPredictionClient
{
    public Task<MlPredictionResponse> PredictAsync(
        MlPredictionRequest request,
        CancellationToken cancellationToken);
}