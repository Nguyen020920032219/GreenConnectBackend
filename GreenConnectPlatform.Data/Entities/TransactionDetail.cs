using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class TransactionDetail
{
    public Guid TransactionId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public decimal PricePerUnit { get; set; }
    public string Unit { get; set; } = "kg";
    public float Quantity { get; set; }
    public decimal FinalPrice { get; set; }
    public ItemTransactionType Type { get; set; }
    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
    public virtual Transaction Transaction { get; set; } = null!;
}