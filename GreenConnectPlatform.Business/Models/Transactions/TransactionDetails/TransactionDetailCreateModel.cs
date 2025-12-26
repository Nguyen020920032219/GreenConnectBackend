using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc.")]
    public Guid ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "PricePerUnit là bắt buộc.")]
    public decimal PricePerUnit { get; set; }

    [Required(ErrorMessage = "Unit là bắt buộc.")]
    public string Unit { get; set; } = "kg";

    [Required(ErrorMessage = "Quantity là bắt buộc.")]
    public float Quantity { get; set; }
}