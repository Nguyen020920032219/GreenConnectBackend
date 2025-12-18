namespace GreenConnectPlatform.Data.Entities;

public class ScrapCategory
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = "kg"; 
    public string? ImageUrl { get; set; }
    public virtual ICollection<ScrapPostDetail> ScrapPostDetails { get; set; } = new List<ScrapPostDetail>();

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}