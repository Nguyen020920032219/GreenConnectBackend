using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferOveralForHouseModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }
    public ScrapPostModel ScrapPost { get; set; } = new();

    public UserViewModel Collector { get; set; } = new();

    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}