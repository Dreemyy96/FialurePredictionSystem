using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.ServiceLayer.Services.MetricService;

public interface IMetricService
{
    public Task AddMetricAsync(CreateMetricRequest request, CancellationToken cancellationToken);

    public Task<IReadOnlyCollection<MetricDashboardResponse>> GetMetricsByEquipmentAsync(Guid equipmentId, int limit = 100,
        CancellationToken cancellationToken = default);
}