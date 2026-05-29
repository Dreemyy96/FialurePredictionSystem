using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.MetricService;

public class MetricService : IMetricService
{
    private readonly FailureSystemDbContext _dbContext;

    public MetricService(FailureSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddMetricAsync(CreateMetricRequest request, CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .FirstOrDefaultAsync(e =>
                e.Id == request.EquipmentId &&
                e.AgentId == request.AgentId &&
                e.IsActive, cancellationToken);

        if (equipment is null)
            throw new InvalidOperationException("Оборудование не найдено или AgentId не совпадает.");

        var metric = new Metric(
            equipment.Id,
            request.AgentId,
            request.Hostname,
            request.TimestampUtc,
            request.CpuUsagePercent,
            request.RamUsagePercent,
            request.DiskUsagePercent,
            request.FreeDiskSpaceGb,
            request.TemperatureCelsius,
            request.ErrorCount,
            request.State,
            request.UptimeHours);

        _dbContext.Metrics.Add(metric);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<MetricDashboardResponse>> GetMetricsByEquipmentAsync(Guid equipmentId, int limit = 100,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Metrics
            .Where(m => m.EquipmentId == equipmentId)
            .OrderByDescending(m => m.TimestampUtc)
            .Take(limit)
            .Select(m => MetricDashboardResponse.Create(m))
            .ToListAsync(cancellationToken);
    }
}