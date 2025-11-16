using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferOveralForCollectorModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }
    
    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}