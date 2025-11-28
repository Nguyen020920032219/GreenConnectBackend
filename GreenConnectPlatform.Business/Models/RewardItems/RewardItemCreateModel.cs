using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.RewardItems;

public class RewardItemCreateModel
{
    [Required(ErrorMessage = "ItemName là bắt buộc.")]
    public string ItemName { get; set; } = null!;

    [Required(ErrorMessage = "Description là bắt buộc.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "PointsCost là bắt buộc.")]
    [Range(1, int.MaxValue, ErrorMessage = "PointsCost phải lớn hơn 0.")]
    public int PointsCost { get; set; }
}