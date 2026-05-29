using System;

namespace FailurePredictionSystem.Agent.Models.ConfigModels;

public class AgentSetting
{
    public Guid AgentId { get; set; }

    public Guid EquipmentId { get; set; }

    public string AgentToken { get; set; } = string.Empty;

    public string ApiUrl { get; set; } = string.Empty;

    public int IntervalSeconds { get; set; } = 10;

    public string DiskPath { get; set; } = "C:\\";
}