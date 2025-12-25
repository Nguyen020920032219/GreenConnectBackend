using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc")]
    public Guid ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "PricePerUnit là bắt buộc")]
    public decimal PricePerUnit { get; set; }

    [Required(ErrorMessage = "Unit là bắt buộc")]
    public string Unit { get; set; } = "kg";
}