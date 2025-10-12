using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class ScrapPost
{
    public Guid ScrapPostId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public string? AvailableTimeRange { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid HouseholdId { get; set; }
    public virtual User Household { get; set; } = null!;
    public virtual ICollection<ScrapPostDetail> Details { get; set; } = new List<ScrapPostDetail>();
    public virtual ICollection<CollectionOffer> Offers { get; set; } = new List<CollectionOffer>();
}