using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.RecurringScheduleDetails;

public class RecurringScheduleDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc.")]
    public Guid ScrapCategoryId { get; set; }
    [Required(ErrorMessage = "Quantity là bắt buộc.")]
    public double Quantity { get; set; }
    [Required(ErrorMessage = "Unit là bắt buộc.")]
    public string Unit { get; set; } = "kg";

    [Required(ErrorMessage = "AmountDecription là bắt buộc.")]
    public string AmountDescription { get; set; } = null!;
    [Required(ErrorMessage = "Type là bắt buộc.")]
    public ItemTransactionType Type { get; set; } = ItemTransactionType.Sale;
}