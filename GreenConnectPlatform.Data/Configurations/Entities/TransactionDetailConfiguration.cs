using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
{
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
        // 1. Khóa chính phức hợp
        builder.HasKey(x => new { x.TransactionId, x.ScrapCategoryId });

        // 2. Properties
        builder.Property(x => x.Unit)
            .HasMaxLength(20);

        builder.Property(x => x.PricePerUnit)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.FinalPrice)
            .HasColumnType("decimal(18,2)");

        // [NEW]
        builder.Property(x => x.Type)
            .HasDefaultValue(ItemTransactionType.Sale)
            .IsRequired();

        // 3. Quan hệ
        builder.HasOne(x => x.Transaction)
            .WithMany(t => t.TransactionDetails)
            .HasForeignKey(x => x.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ScrapCategory)
            .WithMany()
            .HasForeignKey(x => x.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}