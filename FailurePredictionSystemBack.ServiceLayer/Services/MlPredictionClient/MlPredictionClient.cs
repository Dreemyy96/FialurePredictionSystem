using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using Microsoft.Extensions.Logging;

namespace FailurePredictionSystemBack.ServiceLayer.Services.MlPredictionClient;

public class MlPredictionClient : IMlPredictionClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MlPredictionClient> _logger;

    public MlPredictionClient(
        HttpClient httpClient,
        ILogger<MlPredictionClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<MlPredictionResponse> PredictAsync(
        MlPredictionRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/predict",
                request,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "ML service returned unsuccessful status code: {StatusCode}",
                    response.StatusCode);

                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<MlPredictionResponse>(
                cancellationToken: cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while calling ML prediction service.");
            return null;
        }
    }
}