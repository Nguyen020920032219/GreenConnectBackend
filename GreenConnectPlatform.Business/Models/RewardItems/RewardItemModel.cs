namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RewardItemModel
{
    public int RewardItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public string? Description { get; set; }

    public int PointsCost { get; set; }
}