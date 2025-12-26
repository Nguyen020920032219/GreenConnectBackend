using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailUpdateModel
{
    [Range(0.01, double.MaxValue, ErrorMessage = "PricePerUnit phải lớn hơn 0.")]
    public decimal? PricePerUnit { get; set; }

    public string? Unit { get; set; } = "kg";

    [Range(0.01, float.MaxValue, ErrorMessage = "Quantity phải lớn hơn 0.")]
    public float? Quantity { get; set; }
}