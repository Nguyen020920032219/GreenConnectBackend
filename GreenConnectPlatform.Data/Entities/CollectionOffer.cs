using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class CollectionOffer
{
    public Guid OfferId { get; set; }
    public decimal ProposedPrice { get; set; }
    public OfferStatus Status { get; set; } = OfferStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ScrapPostId { get; set; }
    public Guid ScrapCollectorId { get; set; }
    public virtual ScrapPost ScrapPost { get; set; } = null!;
    public virtual User ScrapCollector { get; set; } = null!;
    public virtual Transaction? Transaction { get; set; }
    public virtual ICollection<ScrapPostDetail> OfferedItems { get; set; } = new List<ScrapPostDetail>();
}