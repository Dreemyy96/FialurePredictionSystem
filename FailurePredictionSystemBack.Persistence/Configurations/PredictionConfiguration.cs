using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class PredictionConfiguration : IEntityTypeConfiguration<Prediction>
{
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EquipmentId)
            .IsRequired();

        builder.Property(x => x.MetricId)
            .IsRequired();

        builder.Property(x => x.PredictedState)
            .IsRequired();

        builder.Property(x => x.NormalProbability)
            .IsRequired();

        builder.Property(x => x.WarningProbability)
            .IsRequired();

        builder.Property(x => x.CriticalProbability)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Metric)
            .WithOne(x => x.Prediction)
            .HasForeignKey<Prediction>(x => x.MetricId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.Predictions)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(x => x.EquipmentId);
        builder.HasIndex(x => x.MetricId)
            .IsUnique();
    }
}