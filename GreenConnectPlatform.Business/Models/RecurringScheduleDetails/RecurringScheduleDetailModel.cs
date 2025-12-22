using GreenConnectPlatform.Business.Models.ScrapCategories;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.RecurringScheduleDetails;

public class RecurringScheduleDetailModel
{
    public Guid Id { get; set; }
    public Guid RecurringScheduleId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public ScrapCategoryModel? ScrapCategory { get; set; }
    public double Quantity { get; set; }
    public string Unit { get; set; } = "kg";
    public string? AmountDescription { get; set; }
    public ItemTransactionType Type { get; set; } = ItemTransactionType.Sale;
}