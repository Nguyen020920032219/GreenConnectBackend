using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(e => e.ProfileId);
        builder.Property(e => e.ProfileId).ValueGeneratedNever();

        builder.HasIndex(e => e.UserId).IsUnique();
        builder.HasIndex(e => e.Location).HasMethod("gist");

        builder.Property(e => e.Address).HasMaxLength(255);
        builder.Property(e => e.BankCode).HasMaxLength(20);
        builder.Property(e => e.BankAccountNumber).HasMaxLength(50);
        builder.Property(e => e.BankAccountName).HasMaxLength(100);
        builder.Property(e => e.Gender).HasConversion<string>();
        builder.Property(e => e.Location).HasColumnType("geometry(Point,4326)");
        builder.Property(e => e.PointBalance).HasDefaultValue(200);
        builder.Property(e => e.CreditBalance).HasDefaultValue(0);
        builder.HasOne(d => d.User)
            .WithOne(p => p.Profile)
            .HasForeignKey<Profile>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}