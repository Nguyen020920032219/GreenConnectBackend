using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferModel
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }

    public OfferStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OfferDetailModel> OfferDetails { get; set; } = new();

    public List<ScheduleProposalModel> ScheduleProposals { get; set; } = new();
}