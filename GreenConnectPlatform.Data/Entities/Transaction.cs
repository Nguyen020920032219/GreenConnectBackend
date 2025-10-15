using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }

    public Guid HouseholdId { get; set; }

    public Guid ScrapCollectorId { get; set; }

    public Guid OfferId { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Scheduled;

    public DateTime? ScheduledTime { get; set; }

    public DateTime? CheckInTime { get; set; }

    public string? CheckInSelfieUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ChatRoom? ChatRoom { get; set; }

    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User Household { get; set; } = null!;

    public virtual CollectionOffer Offer { get; set; } = null!;

    public virtual ICollection<ScheduleProposal> ScheduleProposals { get; set; } = new List<ScheduleProposal>();

    public virtual User ScrapCollector { get; set; } = null!;

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}