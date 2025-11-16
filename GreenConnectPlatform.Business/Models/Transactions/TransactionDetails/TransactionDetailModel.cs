namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailModel
{
    public Guid TransactionId { get; set; }

    public int ScrapCategoryId { get; set; }

    public decimal PricePerUnit { get; set; }

    public string Unit { get; set; } = "kg";

    public float Quantity { get; set; }

    public decimal FinalPrice { get; set; }
}