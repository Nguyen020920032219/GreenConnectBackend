using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostOverralModel
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public UserViewModel Household { get; set; } = new();
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? AvailableTimeRange { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public PostStatus Status { get; set; }
}