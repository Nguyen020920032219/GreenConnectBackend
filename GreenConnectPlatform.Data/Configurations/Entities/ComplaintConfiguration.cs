using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasKey(e => e.ComplaintId);
        builder.Property(e => e.ComplaintId).ValueGeneratedNever();
        builder.Property(e => e.Status).HasConversion<string>();

        builder.HasOne(d => d.Transaction)
            .WithMany(p => p.Complaints)
            .HasForeignKey(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Complainant)
            .WithMany(p => p.ComplaintComplainants)
            .HasForeignKey(d => d.ComplainantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Accused)
            .WithMany(p => p.ComplaintAccuseds)
            .HasForeignKey(d => d.AccusedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}