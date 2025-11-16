using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailUpdateModel
{
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit must be greater than zero.")]
    public decimal? PricePerUnit { get; set; }

    public string? Unit { get; set; } = "kg";

    [Range(0.01, float.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public float? Quantity { get; set; }
}