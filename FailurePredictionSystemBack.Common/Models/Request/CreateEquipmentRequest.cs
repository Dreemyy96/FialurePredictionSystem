using System;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Common.Models.Request;

public class CreateEquipmentRequest
{
    public Guid AgentId { get; init; }
    public string AgentToken { get; init; }
    public string Name { get; init; }
    public string Hostname { get; init; }
    public EquipmentType Type { get; init; }
    public string Location { get; init; }
}