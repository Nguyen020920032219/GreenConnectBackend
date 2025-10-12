using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenConnectPlatform.Data.Entities;

public class UserRewardRedemption
{
    public Guid RedemptionId { get; set; }
    public DateTime RedemptionDate { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public int RewardItemId { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual RewardItem RewardItem { get; set; } = null!;
}