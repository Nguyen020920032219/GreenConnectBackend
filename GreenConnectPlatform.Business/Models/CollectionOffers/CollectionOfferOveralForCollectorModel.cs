using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferOveralForCollectorModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }
    public ScrapPostModel ScrapPost { get; set; } = new();

    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}