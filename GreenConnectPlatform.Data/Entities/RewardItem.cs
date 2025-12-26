using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Data.Entities;

public class RewardItem
{
    [Key] public int RewardItemId { get; set; }

    public string ItemName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int PointsCost { get; set; }
    public string? ImageUrl { get; set; }

    // Loại quà: "Credit" (Cộng lượt) hoặc "Package" (Gán gói cước)
    public string Type { get; set; } = "Credit";

    // Giá trị: 
    // - Nếu Type="Credit" -> Value="10" (Cộng 10 lượt)
    // - Nếu Type="Package" -> Value="GUID_CUA_GOI_PRO" (Gán gói Pro)
    public string Value { get; set; } = "0";

    public virtual ICollection<UserRewardRedemption> UserRewardRedemptions { get; set; } =
        new List<UserRewardRedemption>();
}