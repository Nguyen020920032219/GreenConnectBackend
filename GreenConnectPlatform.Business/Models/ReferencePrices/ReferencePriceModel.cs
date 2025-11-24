using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.ReferencePrices;

public class ReferencePriceModel
{
    public Guid ReferencePriceId { get; set; }
    public int ScrapCategoryId { get; set; }
    public ScrapCategoryModel ScrapCategory { get; set; } = new();
    public decimal PricePerKg { get; set; }
    public DateTime LastUpdated { get; set; }
    public Guid UpdatedByAdminId { get; set; }
    public UserViewModel UpdatedByAdmin { get; set; } = new();
}