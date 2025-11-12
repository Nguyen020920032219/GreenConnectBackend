namespace GreenConnectPlatform.Data.Entities;

public class TransactionDetail
{
    public Guid TransactionId { get; set; }
    public int ScrapCategoryId { get; set; }

    public decimal PricePerUnit { get; set; }

    public string Unit { get; set; } = "kg";

    public float Quantity { get; set; }

    public decimal FinalPrice { get; set; }

    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
    public virtual Transaction Transaction { get; set; } = null!;
}