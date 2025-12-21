using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailModel
{
    public Guid ScrapCategoryId { get; set; }
    public ScrapCategoryModel ScrapCategory { get; set; } = new();

    public string? AmountDescription { get; set; }

    public string? ImageUrl { get; set; }

    public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;

    public ItemTransactionType Type { get; set; }
}