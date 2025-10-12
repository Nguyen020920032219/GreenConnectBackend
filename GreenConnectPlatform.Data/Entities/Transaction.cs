using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Scheduled;
    public DateTime? ScheduledTime { get; set; }
    public DateTime? CheckInTime { get; set; }
    public string? CheckInSelfieUrl { get; set; }
    public float? FinalWeight { get; set; }
    public decimal? FinalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid HouseholdId { get; set; }
    public Guid ScrapCollectorId { get; set; }
    public Guid OfferId { get; set; }
    public virtual User Household { get; set; } = null!;
    public virtual User ScrapCollector { get; set; } = null!;
    public virtual CollectionOffer Offer { get; set; } = null!;
    public virtual Feedback? Feedback { get; set; }
    public virtual ICollection<Complaint> Complaints { get; set; } = new List<Complaint>();
    public virtual ChatRoom? ChatRoom { get; set; }
}