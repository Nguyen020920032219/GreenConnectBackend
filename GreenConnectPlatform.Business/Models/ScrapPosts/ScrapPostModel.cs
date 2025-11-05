using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostModel
{
    public Guid ScrapPostId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public string? AvailableTimeRange { get; set; }
    public PostStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid HouseholdId { get; set; }
    public List<ScrapPostDetailModel> ScrapPostDetails { get; set; } = new();
}