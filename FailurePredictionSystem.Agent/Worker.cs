using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystem.Agent.Models.ConfigModels;
using FailurePredictionSystem.Agent.Services;
using FailurePredictionSystem.Agent.Services.MetricCollector;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FailurePredictionSystem.Agent;

public class Worker : BackgroundService
{
    private readonly IMetricCollector _metricCollector;
    private readonly AgentSetting _agentSetting;
    private readonly ILogger<Worker> _logger;
    private readonly MetricSender _metricSender;

    public Worker(
        ILogger<Worker> logger,
        IMetricCollector metricCollector,
        IOptions<AgentSetting> agentSetting,
        MetricSender metricSender)
    {
        _logger = logger;
        _metricSender = metricSender;
        _agentSetting = agentSetting.Value;
        _metricCollector = metricCollector;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            $"Metric agent запущен. AgentId = {_agentSetting.AgentId}, EquipmentId = {_agentSetting.EquipmentId}");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var payload = await _metricCollector.CollectAsync(stoppingToken);

                _logger.LogInformation($"Метрики собраны: CPU={payload.CpuUsagePercent}%, " +
                                       $"RAM={payload.RamUsagePercent}%, " +
                                       $"Disk ={payload.DiskUsagePercent}%, " +
                                       $"Temp = {payload.TemperatureCelsius}C, " +
                                       $"Errors={payload.ErrorCount}");

                await _metricSender.SendAsync(payload, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Metric agent остановлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошикба во время сбора или отправки метрик.");
            }

            await Task.Delay(TimeSpan.FromSeconds(_agentSetting.IntervalSeconds), stoppingToken);
        }
    }
}