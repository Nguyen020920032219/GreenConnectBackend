using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Bussiness.Models.ScrapPosts;

public class ScrapPostRequest
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; } = null!;
    [Required(ErrorMessage = "AvailableTimeRange is required.")]
    public string? AvailableTimeRange { get; set; }
    [JsonIgnore]
    public PostStatus Status { get; private set; } = PostStatus.Open;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Guid HouseholdId { get; set; }
}