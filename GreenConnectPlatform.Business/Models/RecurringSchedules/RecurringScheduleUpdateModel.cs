using GreenConnectPlatform.Business.Models.ScrapPosts;

namespace GreenConnectPlatform.Business.Models.RecurringSchedules;

public class RecurringScheduleUpdateModel
{
    public string? Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; } = null!;
    public LocationModel? Location { get; set; }
    public bool? MustTakeAll { get; set; } = false;
    public int? DayOfWeek { get; set; }
    public TimeOnly? PreferredTime { get; set; }
}