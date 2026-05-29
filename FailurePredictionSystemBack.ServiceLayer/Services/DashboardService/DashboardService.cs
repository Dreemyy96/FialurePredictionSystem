using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.DashboardService;

public class DashboardService : IDashboardService
{
    private readonly FailureSystemDbContext _dbContext;

    public DashboardService(FailureSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EquipmentDashboardResponse> GetEquipmentDashboardAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .AsNoTracking()
            .Where(x => x.Id == equipmentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (equipment is null)
            return null;

        var latestMetric = await _dbContext.Metrics
            .AsNoTracking()
            .Where(x => x.EquipmentId == equipmentId)
            .OrderByDescending(x => x.TimestampUtc)
            .Select(x => MetricDashboardResponse.Create(x))
            .FirstOrDefaultAsync(cancellationToken);

        var latestPrediction = await _dbContext.Predictions
            .AsNoTracking()
            .Where(x => x.EquipmentId == equipmentId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => PredictionDashboardResponse.Create(x))
            .FirstOrDefaultAsync(cancellationToken);

        var metricsCount = await _dbContext.Metrics
            .AsNoTracking()
            .CountAsync(x => x.EquipmentId == equipmentId, cancellationToken);

        var predictionsCount = await _dbContext.Predictions
            .AsNoTracking()
            .CountAsync(x => x.EquipmentId == equipmentId, cancellationToken);

        var normalPredictionsCount = await _dbContext.Predictions
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictedState == EquipmentState.Normal,
                cancellationToken);

        var warningPredictionsCount = await _dbContext.Predictions
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictedState == EquipmentState.Warning,
                cancellationToken);

        var criticalPredictionsCount = await _dbContext.Predictions
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictedState == EquipmentState.Critical,
                cancellationToken);

        var pendingMetricsCount = await _dbContext.Metrics
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictionStatus == PredictionStatus.Pending,
                cancellationToken);

        var processedMetricsCount = await _dbContext.Metrics
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictionStatus == PredictionStatus.Processed,
                cancellationToken);

        var failedMetricsCount = await _dbContext.Metrics
            .AsNoTracking()
            .CountAsync(
                x => x.EquipmentId == equipmentId &&
                     x.PredictionStatus == PredictionStatus.Failed,
                cancellationToken);

        return EquipmentDashboardResponse.Create(equipment, latestMetric, latestPrediction, metricsCount,
            predictionsCount, normalPredictionsCount, warningPredictionsCount, criticalPredictionsCount,
            pendingMetricsCount, processedMetricsCount, failedMetricsCount);
        ;
    }
}