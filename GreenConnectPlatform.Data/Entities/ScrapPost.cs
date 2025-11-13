using GreenConnectPlatform.Data.Enums;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapPost
{
    public Guid ScrapPostId { get; set; }

    public Guid HouseholdId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public string? AvailableTimeRange { get; set; }

    public PostStatus Status { get; set; } = PostStatus.Open;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Point? Location { get; set; }

    public bool MustTakeAll { get; set; } = false;

    public virtual ICollection<CollectionOffer> CollectionOffers { get; set; } = new List<CollectionOffer>();

    public virtual User Household { get; set; } = null!;

    public virtual ICollection<ScrapPostDetail> ScrapPostDetails { get; set; } = new List<ScrapPostDetail>();
}