using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class EquipmentNotificationSubscriptionConfiguration : IEntityTypeConfiguration<EquipmentNotificationSubscription>
{
    public void Configure(EntityTypeBuilder<EquipmentNotificationSubscription> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EquipmentId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.IsInAppEnabled)
            .IsRequired();

        builder.Property(x => x.IsEmailEnabled)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Equipment)
            .WithMany(x => x.NotificationSubscriptions)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.EquipmentNotificationSubscriptions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.EquipmentId, x.UserId })
            .IsUnique();

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.EquipmentId);
    }
}