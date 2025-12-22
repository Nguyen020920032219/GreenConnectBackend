namespace GreenConnectPlatform.Business.Models.RecurringSchedules;

public class RecurringScheduleOverallModel
{
    public Guid Id { get; set; }
    public int DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime LastRunDate { get; set; }
    public DateTime CreatedAt { get; set; }
}