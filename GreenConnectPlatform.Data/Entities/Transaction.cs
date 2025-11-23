using GreenConnectPlatform.Data.Enums;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid HouseholdId { get; set; }
    public Guid ScrapCollectorId { get; set; }
    public Guid OfferId { get; set; }
    public TransactionStatus Status { get; set; }
    public TransactionPaymentMethod? PaymentMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? ScheduledTime { get; set; }
    public DateTime? CheckInTime { get; set; }
    public Point? CheckInLocation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual User? Household { get; set; }
    public virtual User? ScrapCollector { get; set; }
    public virtual CollectionOffer? Offer { get; set; }
    public virtual ICollection<TransactionDetail>? TransactionDetails { get; set; }
    public virtual ChatRoom? ChatRoom { get; set; }
    public virtual ICollection<Complaint>? Complaints { get; set; }
    public virtual ICollection<Feedback>? Feedbacks { get; set; }
}