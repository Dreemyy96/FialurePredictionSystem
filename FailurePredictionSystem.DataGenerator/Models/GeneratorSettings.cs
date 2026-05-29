using System;

namespace FailurePredictionSystem.DataGenerator.Models;

public class GeneratorSettings
{
    public Guid AgentId { get; init; }

    public Guid EquipmentId { get; init; }

    public string AgentToken { get; init; }

    public string ApiUrl { get; init; }

    public string Hostname { get; init; }

    public int TotalRecords { get; init; }

    public int DelayMs { get; init; }
    public bool IncludeState { get; init; }
}