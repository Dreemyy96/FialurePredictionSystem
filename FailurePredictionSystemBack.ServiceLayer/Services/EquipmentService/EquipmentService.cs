using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FailurePredictionSystemBack.Common.Models.Request;
using FailurePredictionSystemBack.Common.Models.Response;
using FailurePredictionSystemBack.Core.Models;
using FailurePredictionSystemBack.Persistence;
using FailurePredictionSystemBack.ServiceLayer.Services.CurrentUserService;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.ServiceLayer.Services.EquipmentService;

public class EquipmentService : IEquipmentService
{
    private readonly FailureSystemDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public EquipmentService(
        FailureSystemDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request,
        CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Equipments
            .AnyAsync(e => e.AgentId == request.AgentId, cancellationToken);

        if (exists)
            throw new InvalidOperationException("Оборудование с таким AgentId уже существует.");

        var equipment = new Equipment(
            request.AgentId,
            HashToken(request.AgentToken),
            request.Name,
            request.Hostname,
            request.Type,
            request.Location);
        
        var currentUserId = _currentUserService.UserId;

        if (currentUserId is not null)
        {
            equipment.SetCreatedBy(currentUserId.Value);
        }

        _dbContext.Equipments.Add(equipment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return EquipmentResponse.Create(equipment);
    }

    public async Task<IReadOnlyCollection<EquipmentResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Equipments
            .OrderBy(e => e.Id)
            .Select(e => EquipmentResponse.Create(e))
            .ToListAsync(cancellationToken);
    }

    public async Task<EquipmentResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return equipment is null ? null : EquipmentResponse.Create(equipment);
    }

    public async Task<bool> ValidateAgentAsync(Guid agentId, string token, CancellationToken cancellationToken)
    {
        var tokenHash = HashToken(token);

        return await _dbContext.Equipments
            .AnyAsync(e =>
                e.AgentId == agentId &&
                e.AgentTokenHash == tokenHash &&
                e.IsActive, cancellationToken);
    }

    private static string HashToken(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}