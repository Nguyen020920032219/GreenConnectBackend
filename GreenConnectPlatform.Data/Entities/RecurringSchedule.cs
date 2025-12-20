using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Entities;

public class RecurringSchedule
{
    public Guid Id { get; set; }
    public Guid HouseholdId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public Point? Location { get; set; }
    public bool MustTakeAll { get; set; } = false;

    public int DayOfWeek { get; set; }
    public TimeOnly PreferredTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastRunDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual User Household { get; set; } = null!;

    public virtual ICollection<RecurringScheduleDetail> ScheduleDetails { get; set; } =
        new List<RecurringScheduleDetail>();
}