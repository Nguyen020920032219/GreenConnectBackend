using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }
    public ScrapPostModel ScrapPost { get; set; } = new();

    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OfferDetailModel> OfferDetails { get; set; } = new();
    public Guid TimeSlotId { get; set; }
    public ScrapPostTimeSlotModel TimeSlot { get; set; }

}