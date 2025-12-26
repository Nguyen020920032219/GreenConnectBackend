using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;
using GreenConnectPlatform.Business.Models.ScrapPosts;

namespace GreenConnectPlatform.Business.Models.RecurringSchedules;

public class RecurringScheduleCreateModel
{
    [Required(ErrorMessage = "Title là bắt buộc.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description là bắt buộc.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Address là bắt buộc.")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "Location là bắt buộc.")]
    public LocationModel Location { get; set; }

    [Required(ErrorMessage = "MustTakeAll là bắt buộc.")]
    public bool MustTakeAll { get; set; } = false;

    [Required(ErrorMessage = "DayOfWeek là bắt buộc.")]
    public int DayOfWeek { get; set; }

    [Required(ErrorMessage = "StartTime là bắt buộc.")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "EndTime là bắt buộc.")]
    public TimeOnly EndTime { get; set; }

    public List<RecurringScheduleDetailCreateModel> ScheduleDetails { get; set; } = new();
}