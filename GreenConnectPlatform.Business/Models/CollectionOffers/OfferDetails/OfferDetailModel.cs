using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

public class OfferDetailModel
{
    public Guid OfferDetailId { get; set; }
    public Guid CollectionOfferId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public ScrapCategoryModel ScrapCategory { get; set; } = new();
    public decimal PricePerUnit { get; set; }
    public string? Unit { get; set; }
    public ItemTransactionType Type { get; set; }
}