using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ReferencePriceConfiguration : IEntityTypeConfiguration<ReferencePrice>
{
    public void Configure(EntityTypeBuilder<ReferencePrice> builder)
    {
        builder.HasKey(e => e.ReferencePriceId);
        builder.Property(e => e.PricePerKg).HasPrecision(18, 2);

        builder.HasOne(d => d.ScrapCategory)
            .WithMany()
            .HasForeignKey(d => d.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.UpdatedByAdmin)
            .WithMany()
            .HasForeignKey(d => d.UpdatedByAdminId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}