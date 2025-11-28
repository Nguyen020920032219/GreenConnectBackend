using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RewardItemUpdateModel
{
    public string? ItemName { get; set; } = null!;
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "PointsCost phải lớn hơn 0.")]
    public int? PointsCost { get; set; }
}