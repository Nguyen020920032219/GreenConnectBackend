using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

public class TransactionDetailModel
{
    public Guid TransactionId { get; set; }

    public Guid ScrapCategoryId { get; set; }
    public ScrapCategoryModel ScrapCategory { get; set; } = new();

    public decimal PricePerUnit { get; set; }

    public string Unit { get; set; } = "kg";

    public float Quantity { get; set; }

    public decimal FinalPrice { get; set; }

    public ItemTransactionType Type { get; set; }
}