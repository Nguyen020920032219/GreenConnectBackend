using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostTimeSlotConfiguration : IEntityTypeConfiguration<ScrapPostTimeSlot>
{
    public void Configure(EntityTypeBuilder<ScrapPostTimeSlot> builder)
    {
        builder.HasKey(x => x.Id);

        // Map DateOnly và TimeOnly sang Postgres
        builder.Property(x => x.SpecificDate).HasColumnType("date").IsRequired();
        builder.Property(x => x.StartTime).HasColumnType("time").IsRequired();
        builder.Property(x => x.EndTime).HasColumnType("time").IsRequired();
        
        builder.Property(x => x.IsBooked).HasDefaultValue(false);

        // Index để tìm lịch nhanh
        builder.HasIndex(x => x.SpecificDate);
    }
}