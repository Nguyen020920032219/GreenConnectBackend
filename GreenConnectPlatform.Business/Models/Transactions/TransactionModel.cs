using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Transactions;

public class TransactionModel
{
    public Guid TransactionId { get; set; }

    public Guid HouseholdId { get; set; }
    public UserViewModel Household { get; set; } = new();

    public Guid ScrapCollectorId { get; set; }
    public UserViewModel ScrapCollector { get; set; } = new();

    public Guid OfferId { get; set; }
    public CollectionOfferModel Offer { get; set; } = new();

    public TransactionStatus Status { get; set; }

    public DateTime? ScheduledTime { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<TransactionDetailModel> TransactionDetails { get; set; } = new();
    
    public Decimal TotalPrice { get; set; }
}