using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferCreateModel
{
    [Required(ErrorMessage = "Must have at least one offer detail")]
    [MinLength(1, ErrorMessage = "Must have at least one offer detail")]
    public List<OfferDetailCreateModel> OfferDetails { get; set; } = new();
    public ScheduleProposalCreateModel ScheduleProposal { get; set; }
}