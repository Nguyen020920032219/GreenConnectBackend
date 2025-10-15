namespace GreenConnectPlatform.Data.Entities;

public class UserRewardRedemption
{
    public Guid UserId { get; set; }

    public int RewardItemId { get; set; }

    public DateTime RedemptionDate { get; set; }

    public virtual RewardItem RewardItem { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}