using System.Transactions;

namespace GreenConnectPlatform.Business.Models.Transactions;

public class TransactionOveralModel
{
    public Guid TransactionId { get; set; }

    public Guid HouseholdId { get; set; }

    public Guid ScrapCollectorId { get; set; }

    public Guid OfferId { get; set; }

    public TransactionStatus Status { get; set; }

    public DateTime? ScheduledTime { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}