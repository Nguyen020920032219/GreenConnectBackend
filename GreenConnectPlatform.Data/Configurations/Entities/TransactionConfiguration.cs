using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.TransactionId);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasConversion<string>();

        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.CheckInLocation).HasColumnType("geometry(Point, 4326)");

        builder.HasOne(x => x.TimeSlot)
            .WithMany()
            .HasForeignKey(x => x.TimeSlotId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Household)
            .WithMany()
            .HasForeignKey(x => x.HouseholdId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ScrapCollector)
            .WithMany()
            .HasForeignKey(x => x.ScrapCollectorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Offer)
            .WithMany(o => o.Transactions)
            .HasForeignKey(x => x.OfferId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}