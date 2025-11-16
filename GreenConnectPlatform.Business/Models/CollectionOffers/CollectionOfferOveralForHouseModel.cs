using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferOveralForHouseModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }

    public string? CollectorName { get; set; }

    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}