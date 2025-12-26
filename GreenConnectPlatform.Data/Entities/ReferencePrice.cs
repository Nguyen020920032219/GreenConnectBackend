using System.ComponentModel.DataAnnotations.Schema;

namespace GreenConnectPlatform.Data.Entities;

public class ReferencePrice
{
    public Guid ReferencePriceId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    [Column(TypeName = "decimal(18, 2)")] public decimal PricePerKg { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public Guid UpdatedByAdminId { get; set; }

    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
    public virtual User UpdatedByAdmin { get; set; } = null!;
}