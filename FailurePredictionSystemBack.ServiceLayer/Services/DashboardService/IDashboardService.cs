using System;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.DashboardService;

public interface IDashboardService
{
    public Task<EquipmentDashboardResponse> GetEquipmentDashboardAsync(
        Guid equipmentId,
        CancellationToken cancellationToken);
}