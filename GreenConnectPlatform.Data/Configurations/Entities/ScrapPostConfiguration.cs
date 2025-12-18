using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostConfiguration : IEntityTypeConfiguration<ScrapPost>
{
    public void Configure(EntityTypeBuilder<ScrapPost> builder)
    {
        // 1. Primary Key
        builder.HasKey(x => x.Id);

        // 2. Properties Config
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200); // Giới hạn độ dài tiêu đề

        builder.Property(x => x.Description)
            .HasMaxLength(1000); // Giới hạn mô tả

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        // Enum conversion (Lưu dưới DB là số int cho nhẹ, hoặc string nếu muốn dễ đọc)
        // Ở đây dùng int mặc định, nhưng nên set Default Value
        builder.Property(x => x.Status)
            .HasDefaultValue(PostStatus.Open)
            .IsRequired();

        builder.Property(x => x.MustTakeAll)
            .HasDefaultValue(false);

        // Map Point cho PostGIS
        builder.Property(x => x.Location)
            .HasColumnType("geometry(Point, 4326)"); // Hệ tọa độ GPS chuẩn

        // 3. Relationships
        
        // Post - User (Household)
        builder.HasOne(p => p.Household)
            .WithMany() // User không nhất thiết phải list hết Post trong object User
            .HasForeignKey(p => p.HouseholdId)
            .OnDelete(DeleteBehavior.Restrict); // Xóa User không xóa Post ngay (để lưu history)

        // Post - TimeSlots (QUAN TRỌNG)
        // Khi xóa Post -> Xóa luôn các Slot rảnh (Cascade)
        builder.HasMany(p => p.TimeSlots)
            .WithOne(ts => ts.ScrapPost)
            .HasForeignKey(ts => ts.ScrapPostId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Post - Details
        builder.HasMany(p => p.ScrapPostDetails)
            .WithOne(d => d.ScrapPost)
            .HasForeignKey(d => d.ScrapPostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}