namespace GreenConnectPlatform.Business.Models.ScrapPostTimeSlots;

public class ScrapPostTimeSlotCreateModel
{
    public DateOnly SpecificDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}