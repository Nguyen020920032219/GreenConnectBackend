using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailRequest
{
    [JsonIgnore] public Guid ScrapPostId { get; set; }

    [Required(ErrorMessage = "ScrapCategoryId is required.")]
    public int ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "AmountDescription is required.")]
    public string? AmountDescription { get; set; }

    [Required(ErrorMessage = "ImageUrl is required.")]
    public string? ImageUrl { get; set; }

    [JsonIgnore] public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;
}