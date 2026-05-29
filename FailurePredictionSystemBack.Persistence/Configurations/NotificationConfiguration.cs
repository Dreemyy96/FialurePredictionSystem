using FailurePredictionSystemBack.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FailurePredictionSystemBack.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.AlertId)
            .IsRequired();

        builder.Property(x => x.Channel)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.IsRead)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.SentAtUtc)
            .IsRequired(false);

        builder.Property(x => x.ErrorMessage)
            .IsRequired(false)
            .HasMaxLength(2000);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Alert)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.AlertId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId);

        builder.HasIndex(x => x.AlertId);

        builder.HasIndex(x => x.Channel);

        builder.HasIndex(x => x.Status);

        builder.HasIndex(x => x.IsRead);

        builder.HasIndex(x => x.CreatedAtUtc);
    }
}