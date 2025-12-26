using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class CollectionOfferConfiguration : IEntityTypeConfiguration<CollectionOffer>
{
    public void Configure(EntityTypeBuilder<CollectionOffer> builder)
    {
        builder.HasKey(e => e.CollectionOfferId);
        builder.Property(e => e.CollectionOfferId).ValueGeneratedNever();
        builder.Property(e => e.Status).HasConversion<string>();

        builder.HasOne(x => x.TimeSlot)
            .WithMany()
            .HasForeignKey(x => x.TimeSlotId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.ScrapPost)
            .WithMany(p => p.CollectionOffers)
            .HasForeignKey(d => d.ScrapPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.ScrapCollector)
            .WithMany(p => p.CollectionOffers)
            .HasForeignKey(d => d.ScrapCollectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}