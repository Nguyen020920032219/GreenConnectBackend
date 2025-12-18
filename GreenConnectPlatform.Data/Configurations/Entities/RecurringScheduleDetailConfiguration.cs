using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class RecurringScheduleDetailConfiguration : IEntityTypeConfiguration<RecurringScheduleDetail>
{
    public void Configure(EntityTypeBuilder<RecurringScheduleDetail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Unit).HasMaxLength(50);
        
        builder.Property(x => x.Type)
            .HasDefaultValue(ItemTransactionType.Sale)
            .IsRequired();

        // Quan hệ với Category
        builder.HasOne(x => x.ScrapCategory)
            .WithMany()
            .HasForeignKey(x => x.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict); // Xóa Category không được xóa lịch sử
    }
}