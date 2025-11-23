using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class UserRewardRedemptionConfiguration : IEntityTypeConfiguration<UserRewardRedemption>
{
    public void Configure(EntityTypeBuilder<UserRewardRedemption> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RewardItemId, e.RedemptionDate });
        builder.Property(e => e.RedemptionDate).HasDefaultValueSql("now()");

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserRewardRedemptions)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.RewardItem)
            .WithMany(p => p.UserRewardRedemptions)
            .HasForeignKey(d => d.RewardItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}