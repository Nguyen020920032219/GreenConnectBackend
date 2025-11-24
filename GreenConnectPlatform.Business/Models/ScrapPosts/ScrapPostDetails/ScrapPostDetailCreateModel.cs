using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailCreateModel
{
    [JsonIgnore] public Guid ScrapPostId { get; set; }

    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc.")]
    public int ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "AmountDescription là bắt buộc.")]
    public string? AmountDescription { get; set; }

    [Required(ErrorMessage = "ImageUrl là bắt buộc.")]
    public string? ImageUrl { get; set; }

    [JsonIgnore] public PostDetailStatus Status { get; set; } = PostDetailStatus.Available;
}