using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RewardItemUpdateModel
{
    public string? ItemName { get; set; } = null!;
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PointsCost phải lớn hơn 0.")]
    public int? PointsCost { get; set; }
    
    public string? ImageUrl { get; set; }
    
    // [NEW] Thêm 2 trường này
    public string Type { get; set; } = "Credit"; // Credit / Package
    public string Value { get; set; } = "0";
}