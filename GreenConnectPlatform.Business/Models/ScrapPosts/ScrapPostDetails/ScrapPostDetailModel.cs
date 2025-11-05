using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailModel
{
    public int ScrapCategoryId { get; set; }

    public string? AmountDescription { get; set; }

    public string? ImageUrl { get; set; }

    public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;
}