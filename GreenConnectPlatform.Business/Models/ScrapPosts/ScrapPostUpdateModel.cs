namespace GreenConnectPlatform.Business.Models.ScrapPosts;

public class ScrapPostUpdateModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? AvailableTimeRange { get; set; }
    public bool? MustTakeAll { get; set; }
    public LocationModel? Location { get; set; }
}