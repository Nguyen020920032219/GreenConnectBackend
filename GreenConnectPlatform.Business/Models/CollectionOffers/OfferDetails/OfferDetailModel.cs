using GreenConnectPlatform.Business.Models.ScrapCategories;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailModel
{
    public Guid OfferDetailId { get; set; }
    public Guid CollectionOfferId { get; set; }
    public int ScrapCategoryId { get; set; }
    public ScrapCategoryModel ScrapCategory { get; set; } = new();
    public decimal PricePerUnit { get; set; }
    public string? Unit { get; set; }
}