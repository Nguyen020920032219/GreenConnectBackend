using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class PaymentPackageConfiguration : IEntityTypeConfiguration<PaymentPackage>
{
    public void Configure(EntityTypeBuilder<PaymentPackage> builder)
    {
        builder.HasKey(e => e.PackageId);
        builder.Property(e => e.Price).HasPrecision(18, 2);
        builder.Property(e => e.PackageType).HasConversion<string>();
    }
}