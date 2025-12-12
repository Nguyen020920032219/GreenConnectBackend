using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostTimeSlotConfiguration : IEntityTypeConfiguration<ScrapPostTimeSlot>
{
    public void Configure(EntityTypeBuilder<ScrapPostTimeSlot> builder)
    {
        builder.ToTable("ScrapPostTimeSlots");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StartTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(x => x.EndTime)
            .IsRequired()
            .HasColumnType("time");

        builder.Property(x => x.SpecificDate)
            .HasColumnType("date");

        builder.Property(x => x.ScrapPostId)
            .IsRequired();
    }
}