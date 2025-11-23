using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
{
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
        builder.HasKey(e => new { e.TransactionId, e.ScrapCategoryId });

        builder.Property(e => e.PricePerUnit).HasPrecision(18, 2);
        builder.Property(e => e.FinalPrice).HasPrecision(18, 2);
        builder.Property(e => e.Unit).HasMaxLength(10).HasDefaultValue("kg");

        builder.HasOne(d => d.Transaction)
            .WithMany(p => p.TransactionDetails)
            .HasForeignKey(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.ScrapCategory)
            .WithMany(p => p.TransactionDetails)
            .HasForeignKey(d => d.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}