using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId is required")]
    public int ScrapCategoryId { get; set; }
    [Required(ErrorMessage = "PricePerUnit is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit must be greater than zero")]
    public decimal PricePerUnit { get; set; }
    [Required(ErrorMessage = "Unit is required")]
    public string Unit { get; set; } = "kg";
}