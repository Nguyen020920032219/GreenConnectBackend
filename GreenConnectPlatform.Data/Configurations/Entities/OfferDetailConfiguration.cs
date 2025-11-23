using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class OfferDetailConfiguration : IEntityTypeConfiguration<OfferDetail>
{
    public void Configure(EntityTypeBuilder<OfferDetail> builder)
    {
        builder.HasKey(e => e.OfferDetailId);

        builder.Property(e => e.PricePerUnit).HasPrecision(18, 2);
        builder.Property(e => e.Unit).HasMaxLength(10).HasDefaultValue("kg");

        builder.HasOne(d => d.CollectionOffer)
            .WithMany(p => p.OfferDetails)
            .HasForeignKey(d => d.CollectionOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.ScrapCategory)
            .WithMany()
            .HasForeignKey(d => d.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}