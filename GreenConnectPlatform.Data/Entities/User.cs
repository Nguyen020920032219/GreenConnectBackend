using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Data.Entities;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }

    public string? FullName { get; set; }

    public UserStatus Status { get; set; } = UserStatus.PendingVerification;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public override string? PhoneNumber { get; set; }

    public string? OtpCode { get; set; }

    public DateTime? OtpExpiredAt { get; set; }

    public virtual ICollection<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();

    public virtual ICollection<CollectionOffer> CollectionOffers { get; set; } = new List<CollectionOffer>();

    public virtual CollectorVerificationInfo? CollectorVerificationInfo { get; set; }

    public virtual ICollection<Complaint> ComplaintAccuseds { get; set; } = new List<Complaint>();

    public virtual ICollection<Complaint> ComplaintComplainants { get; set; } = new List<Complaint>();

    public virtual ICollection<Feedback> FeedbackReviewees { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackReviewers { get; set; } = new List<Feedback>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual Profile? Profile { get; set; }

    public virtual ICollection<ScheduleProposal> ScheduleProposals { get; set; } = new List<ScheduleProposal>();

    public virtual ICollection<ScrapPost> ScrapPosts { get; set; } = new List<ScrapPost>();

    public virtual ICollection<Transaction> TransactionHouseholds { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionScrapCollectors { get; set; } = new List<Transaction>();

    public virtual ICollection<UserRewardRedemption> UserRewardRedemptions { get; set; } =
        new List<UserRewardRedemption>();
}