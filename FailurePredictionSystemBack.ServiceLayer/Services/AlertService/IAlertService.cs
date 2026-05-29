using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.AlertService;

public interface IAlertService
{
    public Task<IReadOnlyList<AlertResponse>> GetAllAsync(
        bool? isResolved,
        CancellationToken cancellationToken);

    public Task<IReadOnlyList<AlertResponse>> GetByEquipmentIdAsync(
        Guid equipmentId,
        bool? isResolved,
        CancellationToken cancellationToken);

    public Task<AlertResponse> GetByIdAsync(
        Guid alertId,
        CancellationToken cancellationToken);

    public Task<bool> ResolveAsync(
        Guid alertId,
        CancellationToken cancellationToken);
}