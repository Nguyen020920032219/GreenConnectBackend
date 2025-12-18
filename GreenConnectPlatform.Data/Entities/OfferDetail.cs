using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class OfferDetail
{
    public Guid OfferDetailId { get; set; }
    public Guid CollectionOfferId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Unit { get; set; } = "kg";
    public ItemTransactionType Type { get; set; }
    public virtual CollectionOffer CollectionOffer { get; set; } = null!;
    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
}