using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapPostDetailConfiguration : IEntityTypeConfiguration<ScrapPostDetail>
{
    public void Configure(EntityTypeBuilder<ScrapPostDetail> builder)
    {
        // 1. Khóa chính phức hợp (Composite Key)
        // Một bài đăng chỉ được có 1 dòng cho 1 loại Category (Ví dụ: 1 dòng Giấy, 1 dòng Nhựa)
        builder.HasKey(x => new { x.ScrapPostId, x.ScrapCategoryId });

        // 2. Properties
        builder.Property(x => x.AmountDescription)
            .HasMaxLength(200);

        builder.Property(x => x.ImageUrl)
            .HasMaxLength(500);

        // [NEW] Cấu hình Type cho giá âm/cho tặng
        builder.Property(x => x.Type)
            .HasDefaultValue(ItemTransactionType.Sale)
            .IsRequired();

        // [NEW] Cấu hình Status cho việc Vét đơn (Available/Booked)
        builder.Property(x => x.Status)
            .HasDefaultValue(PostDetailStatus.Available)
            .IsRequired();

        // 3. Quan hệ
        builder.HasOne(x => x.ScrapCategory)
            .WithMany()
            .HasForeignKey(x => x.ScrapCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ với ScrapPost đã cấu hình ở file ScrapPostConfiguration
    }
}