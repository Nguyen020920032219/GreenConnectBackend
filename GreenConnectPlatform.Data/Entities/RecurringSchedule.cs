namespace GreenConnectPlatform.Data.Entities;

public class RecurringSchedule
{
    public Guid Id { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly TimeSlotStart { get; set; }
    public TimeOnly TimeSlotEnd { get; set; }

    public string ScrapCategoryIds { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Guid HouseholdId { get; set; }
    public virtual User Household { get; set; } = null!;
}