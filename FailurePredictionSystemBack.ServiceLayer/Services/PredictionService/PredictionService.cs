using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.PredictionService;

public class PredictionService : IPredictionService
{
    private readonly FailureSystemDbContext _dbContext;

    public PredictionService(FailureSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PredictionResponse>> GetByEquipmentIdAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var predictions = await _dbContext.Predictions
            .AsNoTracking()
            .Where(x => x.EquipmentId == equipmentId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => PredictionResponse.Create(x))
            .ToListAsync(cancellationToken);

        return predictions;
    }

    public async Task<PredictionResponse> GetLatestByEquipmentIdAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var prediction = await _dbContext.Predictions
            .AsNoTracking()
            .Where(x => x.EquipmentId == equipmentId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => PredictionResponse.Create(x))
            .FirstOrDefaultAsync(cancellationToken);

        return prediction;
    }
}