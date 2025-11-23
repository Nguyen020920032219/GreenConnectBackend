using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.TransactionId);
        builder.Property(e => e.TransactionId).ValueGeneratedNever();

        builder.Property(e => e.TotalAmount).HasPrecision(18, 2);
        builder.Property(e => e.Status).HasConversion<string>();
        builder.Property(e => e.PaymentMethod).HasConversion<string>();

        builder.Property(e => e.CheckInLocation).HasColumnType("geometry(Point,4326)");

        builder.HasOne(d => d.Household)
            .WithMany(p => p.TransactionHouseholds)
            .HasForeignKey(d => d.HouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.ScrapCollector)
            .WithMany(p => p.TransactionScrapCollectors)
            .HasForeignKey(d => d.ScrapCollectorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Offer)
            .WithMany(p => p.Transactions)
            .HasForeignKey(d => d.OfferId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}