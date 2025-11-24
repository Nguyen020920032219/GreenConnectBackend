using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class LocationModel
{
    [Required(ErrorMessage = "Longitude is required.")]
    public double? Longitude { get; set; }
    [Required(ErrorMessage = "Latitude is required.")]
    public double? Latitude { get; set; }
}