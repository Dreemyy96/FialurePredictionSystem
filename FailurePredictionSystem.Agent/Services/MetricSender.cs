using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.Agent.Models;
using FailurePredictionSystem.Agent.Models.ConfigModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FailurePredictionSystem.Agent.Services;

public class MetricSender
{
    private readonly HttpClient _httpClient;
    private readonly AgentSetting _agentSetting;
    private readonly ILogger<MetricSender> _logger;

    public MetricSender(
        HttpClient httpClient,
        IOptions<AgentSetting> agentSetting,
        ILogger<MetricSender> logger)
    {
        _httpClient = httpClient;
        _agentSetting = agentSetting.Value;
        _logger = logger;
    }

    public async Task SendAsync(MetricPayload payload, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, _agentSetting.ApiUrl)
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Add("X-Agent-Token", _agentSetting.AgentToken);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Метрики успешно отправлены. AgentId = {payload.AgentId}, " +
                                       $"EquipmentId = {payload.EquipmentId}, " +
                                       $"CPU = {payload.CpuUsagePercent}%, " +
                                       $"RAM = {payload.RamUsagePercent}%, " +
                                       $"Disk = {payload.DiskUsagePercent}%");
            }
            else
            {
                var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

                _logger.LogWarning(
                    $"Ошибка при отправке метрик. StatusCode = {response.StatusCode}, Response = {responseText}");
            }
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось отправить метрики на Api.");
        }
    }
}