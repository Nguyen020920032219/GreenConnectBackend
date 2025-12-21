using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostDetailConfiguration : IEntityTypeConfiguration<ScrapPostDetail>
{
    public void Configure(EntityTypeBuilder<ScrapPostDetail> builder)
    {
        builder.HasKey(x => new { x.ScrapPostId, x.ScrapCategoryId });

        builder.HasOne(x => x.ScrapCategory)
            .WithMany()
            .HasForeignKey(x => x.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.AmountDescription).HasMaxLength(200);
        builder.Property(x => x.ImageUrl).HasMaxLength(500);

        builder.Property(x => x.Unit).HasMaxLength(50).HasDefaultValue("kg").IsRequired();
        builder.Property(x => x.Quantity).IsRequired();

        builder.Property(x => x.Type).HasDefaultValue(ItemTransactionType.Sale);
        builder.Property(x => x.Status).HasDefaultValue(PostDetailStatus.Available);
    }
}