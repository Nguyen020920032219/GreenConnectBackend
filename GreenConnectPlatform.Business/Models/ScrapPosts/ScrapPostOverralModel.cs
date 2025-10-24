using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostOverralModel
{
    public Guid ScrapPostId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AvailableTimeRange { get; set; }
    public PostStatus Status { get; set; }
}