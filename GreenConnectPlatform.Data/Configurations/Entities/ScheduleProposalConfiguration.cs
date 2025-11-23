using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScheduleProposalConfiguration : IEntityTypeConfiguration<ScheduleProposal>
{
    public void Configure(EntityTypeBuilder<ScheduleProposal> builder)
    {
        builder.HasKey(e => e.ScheduleProposalId);
        builder.Property(e => e.ScheduleProposalId).ValueGeneratedNever();
        builder.Property(e => e.Status).HasConversion<string>();

        builder.HasOne(d => d.Offer)
            .WithMany(p => p.ScheduleProposals)
            .HasForeignKey(d => d.CollectionOfferId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Proposer)
            .WithMany(p => p.ScheduleProposals)
            .HasForeignKey(d => d.ProposerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}