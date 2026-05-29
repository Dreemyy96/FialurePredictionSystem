using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;

namespace FailurePredictionSystemBack.ServiceLayer.Services.EquipmentService;

public interface IEquipmentService
{
    public Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<EquipmentResponse>> GetAllAsync(CancellationToken cancellationToken);
    public Task<EquipmentResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<bool> ValidateAgentAsync(Guid agentId, string token, CancellationToken cancellationToken);
}