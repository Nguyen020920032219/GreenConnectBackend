using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public PostStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid HouseholdId { get; set; }
    public UserViewModel Household { get; set; } = new();
    public bool MustTakeAll { get; set; }
    public List<ScrapPostDetailModel> ScrapPostDetails { get; set; } = new();
    public List<ScrapPostTimeSlotModel> TimeSlots { get; set; } = new();
}