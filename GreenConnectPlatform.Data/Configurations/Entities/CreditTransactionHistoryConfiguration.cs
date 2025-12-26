using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class CreditTransactionHistoryConfiguration : IEntityTypeConfiguration<CreditTransactionHistory>
{
    public void Configure(EntityTypeBuilder<CreditTransactionHistory> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Amount).IsRequired();
        builder.Property(e => e.BalanceAfter).IsRequired();
        builder.Property(e => e.Type).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(255);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

        builder.HasIndex(e => e.UserId);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}