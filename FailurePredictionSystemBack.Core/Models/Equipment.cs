using System;
using System.Collections.Generic;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class Equipment
{
    public Guid Id { get; init; }
    public Guid AgentId { get; init; }
    public string AgentTokenHash { get; init; }
    public string Name { get; init; }
    public string Hostname { get; init; }
    public EquipmentType Type { get; init; }
    public string Location { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAtUtc { get; init; }

    public Guid CreatedByUserId { get; private set; }
    public virtual User CreatedByUser { get; private set; }
    public virtual ICollection<EquipmentNotificationSubscription> NotificationSubscriptions { get; private set; }
    public virtual ICollection<Metric> Metrics { get; private set; }
    public virtual ICollection<Prediction> Predictions { get; private set; }
    public virtual ICollection<Alert> Alerts { get; private set; }

    protected Equipment()
    {
    }

    public Equipment(Guid agentId, string agentTokenHash, string name, string hostname, EquipmentType type,
        string location)
    {
        Id = Guid.NewGuid();
        AgentId = agentId;
        AgentTokenHash = agentTokenHash;
        Name = name;
        Hostname = hostname;
        Type = type;
        Location = location;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }
    
    public void SetCreatedBy(Guid userId)
    {
        CreatedByUserId = userId;
    }
}