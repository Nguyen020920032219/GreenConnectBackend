namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RedemptionHistoryModel
{
    public int RewardItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PointsSpent { get; set; } // Số điểm đã tiêu tốn cho món quà này
    public DateTime RedemptionDate { get; set; }
    public string? ImageUrl { get; set; } // Ảnh của món quà
}