using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailCreateModel
{
    [Required(ErrorMessage = "ScrapCategoryId is required.")]
    public int ScrapCategoryId { get; set; }
    [Required(ErrorMessage = "PricePerUnit is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit must be greater than zero.")]
    public decimal PricePerUnit { get; set; }
    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; } = "kg";
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0.01f, float.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public float Quantity { get; set; }
}