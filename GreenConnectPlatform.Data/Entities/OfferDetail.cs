namespace GreenConnectPlatform.Data.Entities;

public class OfferDetail
{
    public Guid OfferDetailId { get; set; }
    public Guid CollectionOfferId { get; set; }
    public int ScrapCategoryId { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Unit { get; set; } = "kg";

    public virtual CollectionOffer CollectionOffer { get; set; } = null!;
    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
}