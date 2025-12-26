using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapPostDetail
{
    public Guid ScrapPostId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; } = "kg";
    public string? AmountDescription { get; set; }
    public string? ImageUrl { get; set; }
    public ItemTransactionType Type { get; set; } = ItemTransactionType.Sale;
    public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;

    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
    public virtual ScrapPost ScrapPost { get; set; } = null!;
}