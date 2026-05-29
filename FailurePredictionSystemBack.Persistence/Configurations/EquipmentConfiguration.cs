using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.AgentId).IsUnique();

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Hostname)
            .HasMaxLength(200);

        builder.Property(e => e.Location)
            .HasMaxLength(200);

        builder.Property(e => e.AgentTokenHash)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(x => x.CreatedByUserId)
            .IsRequired();

        builder.HasOne(x => x.CreatedByUser)
            .WithMany(x => x.CreatedEquipment)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.CreatedByUserId);
    }
}