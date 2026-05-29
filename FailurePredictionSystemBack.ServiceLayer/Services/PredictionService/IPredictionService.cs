using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.PredictionService;

public interface IPredictionService
{
    public Task<IReadOnlyList<PredictionResponse>> GetByEquipmentIdAsync(
        Guid equipmentId,
        CancellationToken cancellationToken);

    public Task<PredictionResponse> GetLatestByEquipmentIdAsync(
        Guid equipmentId,
        CancellationToken cancellationToken);
}