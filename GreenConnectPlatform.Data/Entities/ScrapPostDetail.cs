using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapPostDetail
{
    public Guid ScrapPostId { get; set; }

    public int ScrapCategoryId { get; set; }

    public string? AmountDescription { get; set; }

    public string? ImageUrl { get; set; }

    public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;

    public virtual ScrapCategory ScrapCategory { get; set; } = null!;

    public virtual ScrapPost ScrapPost { get; set; } = null!;

    public virtual ICollection<CollectionOffer> CollectionOffers { get; set; } = new List<CollectionOffer>();
}