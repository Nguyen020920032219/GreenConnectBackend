using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostDetailConfiguration : IEntityTypeConfiguration<ScrapPostDetail>
{
    public void Configure(EntityTypeBuilder<ScrapPostDetail> builder)
    {
        builder.HasKey(e => new { e.ScrapPostId, e.ScrapCategoryId });

        builder.Property(e => e.AmountDescription).HasMaxLength(100);
        builder.Property(e => e.Status).HasConversion<string>();

        builder.HasOne(d => d.ScrapPost)
            .WithMany(p => p.ScrapPostDetails)
            .HasForeignKey(d => d.ScrapPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.ScrapCategory)
            .WithMany(p => p.ScrapPostDetails)
            .HasForeignKey(d => d.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}