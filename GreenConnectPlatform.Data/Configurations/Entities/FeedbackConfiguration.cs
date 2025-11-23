using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(e => e.FeedbackId);
        builder.Property(e => e.FeedbackId).ValueGeneratedNever();

        builder.HasOne(d => d.Transaction)
            .WithMany(p => p.Feedbacks)
            .HasForeignKey(d => d.TransactionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Reviewer)
            .WithMany(p => p.FeedbackReviewers)
            .HasForeignKey(d => d.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.Reviewee)
            .WithMany(p => p.FeedbackReviewees)
            .HasForeignKey(d => d.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}