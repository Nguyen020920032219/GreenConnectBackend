using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.PointHistories;

public class PointHistoryModel
{
    public Guid PointHistoryId { get; set; }
    public Guid UserId { get; set; }
    public UserViewModel User { get; set; } = new();
    public int PointChange { get; set; }
    public string Reason { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}