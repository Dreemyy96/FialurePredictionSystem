using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EquipmentId)
            .IsRequired();

        builder.Property(x => x.PredictionId)
            .IsRequired();

        builder.Property(x => x.Severity)
            .IsRequired();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.IsResolved)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.ResolvedAtUtc)
            .IsRequired(false);

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.Alerts)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Prediction)
            .WithOne(x => x.Alert)
            .HasForeignKey<Alert>(x => x.PredictionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.EquipmentId);

        builder.HasIndex(x => x.PredictionId)
            .IsUnique();

        builder.HasIndex(x => x.IsResolved);

        builder.HasIndex(x => x.Severity);

        builder.HasIndex(x => x.CreatedAtUtc);
    }
}