using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId là bắt buộc.")]
    public Guid ScrapCategoryId { get; set; }

    [Required(ErrorMessage = "PricePerUnit là bắt buộc.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit phải lớn hơn 0.")]
    public decimal PricePerUnit { get; set; }

    [Required(ErrorMessage = "Unit là bắt buộc.")]
    public string Unit { get; set; } = "kg";

    [Required(ErrorMessage = "Quantity là bắt buộc.")]
    [Range(0.01f, float.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0.")]
    public float Quantity { get; set; }
}