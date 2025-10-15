namespace GreenConnectPlatform.Data.Entities;

public class TransactionDetail
{
    public Guid TransactionId { get; set; }

    public int ScrapCategoryId { get; set; }

    public float ActualWeight { get; set; }

    public decimal ActualPrice { get; set; }

    public virtual ScrapCategory ScrapCategory { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}