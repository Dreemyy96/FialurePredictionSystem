using FailurePredictionSystemBack.Core.Enums;
using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class MetricConfiguration : IEntityTypeConfiguration<Metric>
{
    public void Configure(EntityTypeBuilder<Metric> builder)
    {
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => m.EquipmentId);

        builder.HasIndex(m => m.AgentId);

        builder.HasIndex(m => m.TimestampUtc);

        builder.Property(m => m.Hostname)
            .HasMaxLength(200);

        builder.Property(m => m.State)
            .IsRequired(false);
        
        builder.Property(x => x.PredictionStatus)
            .IsRequired()
            .HasDefaultValue(PredictionStatus.Pending);
        
        builder.HasIndex(x => x.EquipmentId);
        builder.HasIndex(x => x.PredictionStatus);
        builder.HasIndex(x => x.TimestampUtc);

        builder.HasOne(m => m.Equipment)
            .WithMany(e => e.Metrics)
            .HasForeignKey(m => m.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.PredictionStatus)
            .IsRequired()
            .HasDefaultValue(PredictionStatus.Pending);

        builder.HasIndex(x => x.PredictionStatus);
    }
}