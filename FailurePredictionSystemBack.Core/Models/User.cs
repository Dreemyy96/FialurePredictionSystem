using System;
using System.Collections.Generic;
using FailurePredictionSystemBack.Core.Enums;

namespace FailurePredictionSystemBack.Core.Models;

public class User
{
    public Guid Id { get; init; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FullName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; init; }
    public virtual ICollection<Equipment> CreatedEquipment { get; private set; }

    public virtual ICollection<EquipmentNotificationSubscription> EquipmentNotificationSubscriptions { get; private set; } 

    public virtual ICollection<Notification> Notifications { get; private set; }

    protected User()
    {
    }

    public User(
        string email,
        string passwordHash,
        string fullName,
        UserRole role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}