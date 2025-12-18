using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class RecurringScheduleDetail
{
    public Guid Id { get; set; }
    public Guid RecurringScheduleId { get; set; }
    public Guid ScrapCategoryId { get; set; }
    public double Quantity { get; set; }
    public string? Unit { get; set; }
    public ItemTransactionType Type { get; set; } = ItemTransactionType.Sale;
    public virtual RecurringSchedule RecurringSchedule { get; set; } = null!;
    public virtual ScrapCategory ScrapCategory { get; set; } = null!;
}