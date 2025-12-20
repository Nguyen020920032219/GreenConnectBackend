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
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(500);
        
        builder.Property(x => x.Location).HasColumnType("geometry(Point, 4326)");
        
        builder.Property(x => x.PreferredTime).HasColumnType("time").IsRequired();
        
        builder.Property(x => x.MustTakeAll).HasDefaultValue(false);
        builder.Property(x => x.IsActive).HasDefaultValue(true);

        builder.HasOne(x => x.Household)
            .WithMany()
            .HasForeignKey(x => x.HouseholdId)
            .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(x => x.ScheduleDetails)
            .WithOne(d => d.RecurringSchedule)
            .HasForeignKey(d => d.RecurringScheduleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}