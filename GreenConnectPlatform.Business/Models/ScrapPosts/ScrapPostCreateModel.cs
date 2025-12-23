using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostCreateModel
{
    [Required(ErrorMessage = "Title là bắt buộc.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description là bắt buộc.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Address là bắt buộc.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "AvailableTimeRange là bắt buộc.")]
    public string? AvailableTimeRange { get; set; }

    [JsonIgnore] public PostStatus Status { get; private set; } = PostStatus.Open;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore] public Guid HouseholdId { get; set; }

    [Required(ErrorMessage = "Location là bắt buộc.")]
    public LocationModel Location { get; set; } = null!;

    public bool MustTakeAll { get; set; }

    public List<ScrapPostDetailCreateModel> ScrapPostDetails { get; set; } = new();

    public List<ScrapPostTimeSlotCreateModel> ScrapPostTimeSlots { get; set; } = new();
}