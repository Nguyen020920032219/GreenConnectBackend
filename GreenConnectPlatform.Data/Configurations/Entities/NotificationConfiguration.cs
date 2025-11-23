using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(e => e.NotificationId);
        builder.Property(e => e.NotificationId).ValueGeneratedNever();
        builder.Property(e => e.IsRead).HasDefaultValue(false);
        builder.Property(e => e.EntityType).HasMaxLength(50);

        builder.HasIndex(e => new { e.RecipientId, e.CreatedAt }).IsDescending(false, true);

        builder.HasOne(d => d.Recipient)
            .WithMany(p => p.Notifications)
            .HasForeignKey(d => d.RecipientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}