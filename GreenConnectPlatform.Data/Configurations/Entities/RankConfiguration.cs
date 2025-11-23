using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class RankConfiguration : IEntityTypeConfiguration<Rank>
{
    public void Configure(EntityTypeBuilder<Rank> builder)
    {
        builder.HasKey(e => e.RankId);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(50);

        builder.HasIndex(e => e.MinPoints).IsUnique();
    }
}