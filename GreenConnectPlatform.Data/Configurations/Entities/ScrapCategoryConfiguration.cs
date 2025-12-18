using GreenConnectPlatform.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenConnectPlatform.Data.Configurations.Entities;

public class ScrapCategoryConfiguration : IEntityTypeConfiguration<ScrapCategory>
{
    public void Configure(EntityTypeBuilder<ScrapCategory> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
    }
}