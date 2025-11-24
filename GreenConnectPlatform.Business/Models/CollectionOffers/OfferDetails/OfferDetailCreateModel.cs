using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc")]
    public int ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "PricePerUnit là bắt buộc")]
    [Range(0.1, double.MaxValue, ErrorMessage = "PricePerUnit phải lớn hơn 0")]
    public decimal PricePerUnit { get; set; }

    [Required(ErrorMessage = "Unit là bắt buộc")]
    public string Unit { get; set; } = "kg";
}