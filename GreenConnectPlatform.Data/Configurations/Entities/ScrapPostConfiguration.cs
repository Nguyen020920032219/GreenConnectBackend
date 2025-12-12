using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostConfiguration : IEntityTypeConfiguration<ScrapPost>
{
    public void Configure(EntityTypeBuilder<ScrapPost> builder)
    {
        builder.HasKey(e => e.ScrapPostId);
        builder.Property(e => e.ScrapPostId).ValueGeneratedNever();

        builder.Property(e => e.Title).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Address).HasMaxLength(255).IsRequired();

        builder.Property(e => e.Status).HasConversion<string>();

        builder.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
        builder.HasIndex(e => e.Location).HasMethod("gist");

        builder.HasIndex(e => new { e.Status, e.HouseholdId });

        builder.HasMany(p => p.TimeSlots)
            .WithOne(ts => ts.ScrapPost)
            .HasForeignKey(ts => ts.ScrapPostId);

        builder.HasOne(d => d.Household)
            .WithMany(p => p.ScrapPosts)
            .HasForeignKey(d => d.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}