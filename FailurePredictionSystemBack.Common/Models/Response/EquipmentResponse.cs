using System;
using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;

namespace FailurePredictionSystemBack.Common.Models.Response;

public class EquipmentResponse
{
    public Guid Id { get; }
    public Guid AgentId { get; }
    public string Name { get; }
    public string Hostname { get; }
    public EquipmentType Type { get; }
    public string Location { get; }
    public bool IsActive { get; }
    public DateTime CreatedAtUtc { get; }

    private EquipmentResponse(Equipment equipment)
    {
        Id = equipment.Id;
        AgentId = equipment.AgentId;
        Name = equipment.Name;
        Hostname = equipment.Hostname;
        Type = equipment.Type;
        Location = equipment.Location;
        IsActive = equipment.IsActive;
        CreatedAtUtc = equipment.CreatedAtUtc;
    }

    public static EquipmentResponse Create(Equipment equipment)
    {
        return new EquipmentResponse(equipment);
    }
}