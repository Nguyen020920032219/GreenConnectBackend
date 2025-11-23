using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class PointHistoryConfiguration : IEntityTypeConfiguration<PointHistory>
{
    public void Configure(EntityTypeBuilder<PointHistory> builder)
    {
        builder.HasKey(e => e.PointHistoryId);
        builder.Property(e => e.Reason).HasMaxLength(255);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}