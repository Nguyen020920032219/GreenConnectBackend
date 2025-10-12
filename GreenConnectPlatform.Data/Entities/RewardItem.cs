using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenConnectPlatform.Data.Entities;

public class RewardItem
{
    public int RewardItemId { get; set; }
    public string ItemName { get; set; } = null!;
    public string? Description { get; set; }
    public int PointsCost { get; set; }
    public virtual ICollection<UserRewardRedemption> Redemptions { get; set; } = new List<UserRewardRedemption>();
}