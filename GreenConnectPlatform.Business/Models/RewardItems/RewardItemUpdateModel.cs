namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RewardItemUpdateModel
{
    public string? ItemName { get; set; }
    public string? Description { get; set; }
    public int? PointsCost { get; set; }
    public string? ImageUrl { get; set; }
    public string? Type { get; set; }
    public string? Value { get; set; }
}