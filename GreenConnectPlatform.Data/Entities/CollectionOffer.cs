using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class CollectionOffer
{
    public Guid CollectionOfferId { get; set; }

    public Guid ScrapPostId { get; set; }

    public Guid ScrapCollectorId { get; set; }

    public decimal ProposedPrice { get; set; }

    public OfferStatus Status { get; set; } = OfferStatus.Pending;

    public DateTime CreatedAt { get; set; }

    public virtual User ScrapCollector { get; set; } = null!;

    public virtual ScrapPost ScrapPost { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<ScrapPostDetail> ScrapPostDetails { get; set; } = new List<ScrapPostDetail>();
}