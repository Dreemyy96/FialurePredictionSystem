using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.AlertService;

public class AlertService : IAlertService
{
    private readonly FailureSystemDbContext _dbContext;

    public AlertService(FailureSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AlertResponse>> GetAllAsync(
        bool? isResolved,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Alerts
            .AsNoTracking()
            .AsQueryable();

        if (isResolved.HasValue)
        {
            query = query.Where(x => x.IsResolved == isResolved.Value);
        }

        return await query
            .OrderBy(x => x.IsResolved)
            .ThenByDescending(x => x.CreatedAtUtc)
            .Select(x => AlertResponse.Create(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AlertResponse>> GetByEquipmentIdAsync(
        Guid equipmentId,
        bool? isResolved,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Alerts
            .AsNoTracking()
            .Where(x => x.EquipmentId == equipmentId);

        if (isResolved.HasValue)
        {
            query = query.Where(x => x.IsResolved == isResolved.Value);
        }

        return await query
            .OrderBy(x => x.IsResolved)
            .ThenByDescending(x => x.CreatedAtUtc)
            .Select(x => AlertResponse.Create(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<AlertResponse> GetByIdAsync(
        Guid alertId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Alerts
            .AsNoTracking()
            .Where(x => x.Id == alertId)
            .Select(x => AlertResponse.Create(x))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ResolveAsync(
        Guid alertId,
        CancellationToken cancellationToken)
    {
        var alert = await _dbContext.Alerts
            .FirstOrDefaultAsync(x => x.Id == alertId, cancellationToken);

        if (alert is null)
            return false;

        alert.Resolve();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}