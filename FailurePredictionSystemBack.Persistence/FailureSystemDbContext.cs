using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FailurePredictionSystemBack.Persistence;

public class FailureSystemDbContext : DbContext
{
    public FailureSystemDbContext(DbContextOptions<FailureSystemDbContext> options) : base(options)
    {
    }

    public DbSet<Equipment> Equipments { get; set; }

    public DbSet<Metric> Metrics { get; set; }

    public DbSet<Prediction> Predictions { get; set; }

    public DbSet<Alert> Alerts { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Notification> Notifications { get; set; }

    public DbSet<EquipmentNotificationSubscription> EquipmentNotificationSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FailureSystemDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}