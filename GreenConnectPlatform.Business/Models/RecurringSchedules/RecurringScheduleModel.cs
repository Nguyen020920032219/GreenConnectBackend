using System.Drawing;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;

namespace GreenConnectPlatform.Business.Models.RecurringSchedules;

public class RecurringScheduleModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public bool MustTakeAll { get; set; } = false;

    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastRunDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<RecurringScheduleDetailModel> ScheduleDetails { get; set; } = new ();   
}