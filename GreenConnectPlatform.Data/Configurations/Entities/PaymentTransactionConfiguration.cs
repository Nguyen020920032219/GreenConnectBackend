using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
    {
        builder.HasKey(e => e.PaymentId);
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        builder.Property(e => e.Status).HasConversion<string>();

        builder.Property(e => e.TransactionRef).HasMaxLength(100);
        builder.Property(e => e.VnpTransactionNo).HasMaxLength(100);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}