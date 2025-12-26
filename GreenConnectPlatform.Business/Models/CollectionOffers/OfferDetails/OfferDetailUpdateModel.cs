using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailUpdateModel
{
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit phải lớn hơn 0")]
    public decimal? PricePerUnit { get; set; }

    public string? Unit { get; set; } = "kg";
}