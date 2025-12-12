using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class RecurringScheduleConfiguration : IEntityTypeConfiguration<RecurringSchedule>
{
    public void Configure(EntityTypeBuilder<RecurringSchedule> builder)
    {
        builder.ToTable("RecurringSchedules");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.DayOfWeek)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.TimeSlotStart)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.TimeSlotEnd)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.ScrapCategoryIds)
            .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.HasOne(x => x.Household)
            .WithMany()
            .HasForeignKey(x => x.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}