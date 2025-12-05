using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class CollectorVerificationInfoConfiguration : IEntityTypeConfiguration<CollectorVerificationInfo>
{
    public void Configure(EntityTypeBuilder<CollectorVerificationInfo> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).ValueGeneratedNever();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.IdentityNumber).HasMaxLength(50);
        builder.Property(e => e.FullnameOnId).HasMaxLength(100);
        builder.Property(e => e.PlaceOfOrigin).HasMaxLength(255);
        builder.Property(e => e.IssuedBy).HasMaxLength(200);
        builder.Property(e => e.ReviewerNotes).HasMaxLength(500);

        builder.HasOne(d => d.User)
            .WithOne(p => p.CollectorVerificationInfo)
            .HasForeignKey<CollectorVerificationInfo>(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Reviewer)
            .WithMany()
            .HasForeignKey(d => d.ReviewerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(e => e.Status);

        builder.HasIndex(e => e.IdentityNumber)
            .IsUnique();
    }
}