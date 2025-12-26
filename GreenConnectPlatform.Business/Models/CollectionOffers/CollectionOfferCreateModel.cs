using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

namespace GreenConnectPlatform.Business.Models.CollectionOffers;

public class CollectionOfferCreateModel
{
    [Required(ErrorMessage = "Phải có ít nhất 1 chi tiết đề nghị")]
    [MinLength(1, ErrorMessage = "Phải có ít nhất 1 chi tiết đề nghị")]
    public List<OfferDetailCreateModel> OfferDetails { get; set; } = new();
}