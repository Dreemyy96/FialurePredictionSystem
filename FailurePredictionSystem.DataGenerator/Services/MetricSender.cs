using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.DataGenerator.Models;

namespace FailurePredictionSystem.DataGenerator.Services;

public class MetricSender
{
    private readonly HttpClient _httpClient;
    private readonly GeneratorSettings _settings;

    public MetricSender(HttpClient httpClient, GeneratorSettings settings)
    {
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<bool> SendAsync(MetricPayload payload, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, _settings.ApiUrl)
        {
            Content = JsonContent.Create(payload)
        };

        request.Headers.Add("X-Agent-Token", _settings.AgentToken);

        var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
            return true;

        var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine($"Ошибка отправки. StatusCode={response.StatusCode}, Response={responseText}");

        return false;
    }
}