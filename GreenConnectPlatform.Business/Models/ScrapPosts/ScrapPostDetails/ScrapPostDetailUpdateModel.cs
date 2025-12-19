using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailUpdateModel
{
    public string? AmountDescription { get; set; }
    public string? ImageUrl { get; set; }
    public ItemTransactionType? Type { get; set; }
}