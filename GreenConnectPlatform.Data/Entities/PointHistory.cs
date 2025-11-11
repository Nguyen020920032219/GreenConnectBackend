namespace GreenConnectPlatform.Data.Entities;

public class PointHistory
{
    public Guid PointHistoryId { get; set; }
    public Guid UserId { get; set; }
    public int PointChange { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
}