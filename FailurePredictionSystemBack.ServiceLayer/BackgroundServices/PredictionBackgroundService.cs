using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.ConfigModels;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using FailurePredictionSystemBack.ServiceLayer.Services.MlPredictionClient;
using FailurePredictionSystemBack.ServiceLayer.Services.NotificationService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FailurePredictionSystemBack.ServiceLayer.BackgroundServices;

public class PredictionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PredictionBackgroundService> _logger;
    private readonly MlServiceSettings _settings;

    public PredictionBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<PredictionBackgroundService> logger,
        IOptions<MlServiceSettings> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Prediction background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMetricsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in prediction background service.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_settings.ProcessingIntervalSeconds),
                stoppingToken);
        }
    }

    private async Task ProcessPendingMetricsAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<FailureSystemDbContext>();
        var mlPredictionClient = scope.ServiceProvider.GetRequiredService<IMlPredictionClient>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var metrics = await dbContext.Metrics
            .Where(x => x.PredictionStatus == PredictionStatus.Pending ||
                        x.PredictionStatus == PredictionStatus.Failed)
            .OrderBy(x => x.TimestampUtc)
            .Take(_settings.BatchSize)
            .ToListAsync(cancellationToken);

        if (metrics.Count == 0)
            return;

        _logger.LogInformation(
            "Found {Count} metrics for prediction processing.",
            metrics.Count);

        foreach (var metric in metrics)
        {
            await ProcessMetricAsync(
                dbContext,
                mlPredictionClient,
                notificationService,
                metric,
                cancellationToken);
        }
    }

    private async Task ProcessMetricAsync(
        FailureSystemDbContext dbContext,
        IMlPredictionClient mlPredictionClient,
        INotificationService notificationService,
        Metric metric,
        CancellationToken cancellationToken)
    {
        var request = new MlPredictionRequest
        {
            CpuUsagePercent = metric.CpuUsagePercent,
            RamUsagePercent = metric.RamUsagePercent,
            DiskUsagePercent = metric.DiskUsagePercent,
            FreeDiskSpaceGb = metric.FreeDiskSpaceGb,
            TemperatureCelsius = metric.TemperatureCelsius,
            ErrorCount = metric.ErrorCount,
            UptimeHours = metric.UptimeHours
        };

        var mlResult = await mlPredictionClient.PredictAsync(
            request,
            cancellationToken);

        if (mlResult is null)
        {
            metric.MarkPredictionFailed();

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogWarning(
                "Prediction failed for metric {MetricId}.",
                metric.Id);

            return;
        }

        var predictedState = (EquipmentState)mlResult.PredictedStateCode;

        var prediction = new Prediction(
            equipmentId: metric.EquipmentId,
            metricId: metric.Id,
            predictedState: predictedState,
            normalProbability: mlResult.Probabilities.Normal,
            warningProbability: mlResult.Probabilities.Warning,
            criticalProbability: mlResult.Probabilities.Critical);

        await dbContext.Predictions.AddAsync(prediction, cancellationToken);

        if (predictedState != EquipmentState.Normal)
        {
            var alert = predictedState switch
            {
                EquipmentState.Critical => new Alert(
                    equipmentId: metric.EquipmentId,
                    predictionId: prediction.Id,
                    severity: AlertSeverity.Critical,
                    title: "Обнаружен высокий риск отказа оборудования",
                    message: $"ML-модель спрогнозировала критическое состояние оборудования. " +
                             $"Вероятность критического состояния: {mlResult.Probabilities.Critical:P2}"),
                EquipmentState.Warning => new Alert(
                    equipmentId: metric.EquipmentId,
                    predictionId: prediction.Id,
                    severity: AlertSeverity.Critical,
                    title: "Обнаружен риск отказа оборудования",
                    message: $"ML-модель спрогнозировала предупреждение о состоянии оборудования. " +
                             $"Вероятность критического состояния: {mlResult.Probabilities.Warning:P2}"),
                _ => null
            };

            await dbContext.Alerts.AddAsync(alert, cancellationToken);
            await notificationService.CreateForAlertAsync(
                alert,
                cancellationToken);
        }

        metric.MarkPredictionProcessed();

        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Prediction saved for metric {MetricId}. Predicted state: {PredictedState}",
            metric.Id,
            predictedState);
    }
}