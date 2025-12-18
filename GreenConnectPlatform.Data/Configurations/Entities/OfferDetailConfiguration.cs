using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class OfferDetailConfiguration : IEntityTypeConfiguration<OfferDetail>
{
    public void Configure(EntityTypeBuilder<OfferDetail> builder)
    {
        // 1. Khóa chính (Surrogate Key)
        builder.HasKey(x => x.OfferDetailId);

        // 2. Properties
        builder.Property(x => x.Unit)
            .HasMaxLength(20);
            
        builder.Property(x => x.PricePerUnit)
            .HasColumnType("decimal(18,2)"); // Định dạng tiền tệ

        // [NEW]
        builder.Property(x => x.Type)
            .HasDefaultValue(ItemTransactionType.Sale)
            .IsRequired();

        // 3. Quan hệ
        builder.HasOne(x => x.CollectionOffer)
            .WithMany(o => o.OfferDetails)
            .HasForeignKey(x => x.CollectionOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScrapCategory)
            .WithMany()
            .HasForeignKey(x => x.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}