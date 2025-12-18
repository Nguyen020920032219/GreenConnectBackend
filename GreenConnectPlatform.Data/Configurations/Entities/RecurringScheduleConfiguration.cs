using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class RecurringScheduleConfiguration : IEntityTypeConfiguration<RecurringSchedule>
{
    public void Configure(EntityTypeBuilder<RecurringSchedule> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        
        // Map TimeOnly sang cột 'time' của Postgres
        builder.Property(x => x.PreferredTime).HasColumnType("time").IsRequired();
        
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        // Quan hệ với User
        builder.HasOne(x => x.Household)
            .WithMany()
            .HasForeignKey(x => x.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa User -> Xóa luôn lịch

        // Quan hệ với Details (Cascade Delete)
        builder.HasMany(x => x.ScheduleDetails)
            .WithOne(d => d.RecurringSchedule)
            .HasForeignKey(d => d.RecurringScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}