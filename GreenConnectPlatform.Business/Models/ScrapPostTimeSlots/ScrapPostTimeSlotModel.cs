namespace GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;

public class ScrapPostTimeSlotModel
{
    public Guid Id { get; set; }
    public DateOnly SpecificDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsBooked { get; set; }
    public Guid ScrapPostId { get; set; }
}