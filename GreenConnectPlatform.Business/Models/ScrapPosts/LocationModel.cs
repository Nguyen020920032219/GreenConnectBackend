using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class LocationModel
{
    [Required(ErrorMessage = "Longitude là bắt buộc.")]
    public double? Longitude { get; set; }

    [Required(ErrorMessage = "Latitude là bắt buộc.")]
    public double? Latitude { get; set; }
}