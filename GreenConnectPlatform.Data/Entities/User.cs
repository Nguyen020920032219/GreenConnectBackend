using Microsoft.AspNetCore.Identity;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class User : IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public UserStatus Status { get; set; } = UserStatus.PendingVerification;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual Profile? Profile { get; set; }
    public virtual ICollection<ScrapPost> CreatedScrapPosts { get; set; } = new List<ScrapPost>();
    public virtual ICollection<CollectionOffer> SentOffers { get; set; } = new List<CollectionOffer>();
    public virtual ICollection<Transaction> TransactionsAsHousehold { get; set; } = new List<Transaction>();
    public virtual ICollection<Transaction> TransactionsAsCollector { get; set; } = new List<Transaction>();
    public virtual ICollection<Feedback> ReviewsGiven { get; set; } = new List<Feedback>();
    public virtual ICollection<Feedback> ReviewsReceived { get; set; } = new List<Feedback>();
    public virtual ICollection<Complaint> ComplaintsFiled { get; set; } = new List<Complaint>();
    public virtual ICollection<Complaint> ComplaintsAgainst { get; set; } = new List<Complaint>();
    public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public virtual ICollection<ChatParticipant> ChatRooms { get; set; } = new List<ChatParticipant>();
    public virtual ICollection<UserRewardRedemption> RewardRedemptions { get; set; } = new List<UserRewardRedemption>();
}