using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostCreateModel
{
    [JsonIgnore] public Guid ScrapPostId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "AvailableTimeRange is required.")]
    public string? AvailableTimeRange { get; set; }

    [JsonIgnore] public PostStatus Status { get; private set; } = PostStatus.Open;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore] public Guid HouseholdId { get; set; }

    [Required(ErrorMessage = "Location is required.")]
    public LocationModel Location { get; set; } = null!;

    public bool MustTakeAll { get; set; }

    public List<ScrapPostDetailCreateModel> ScrapPostDetails { get; set; } = new();
}