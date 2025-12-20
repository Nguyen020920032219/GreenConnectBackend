namespace GreenConnectPlatform.Data.Entities;

public class ScrapPostTimeSlot
{
    public Guid Id { get; set; }
    public DateOnly SpecificDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsBooked { get; set; }
    public Guid ScrapPostId { get; set; }
    public virtual ScrapPost ScrapPost { get; set; } = null!;
}