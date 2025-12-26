using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.Feedbacks;

public class FeedbackModel
{
    public Guid FeedbackId { get; set; }

    public Guid TransactionId { get; set; }
    public TransactionModel Transaction { get; set; } = null!;

    public Guid ReviewerId { get; set; }
    public UserViewModel Reviewer { get; set; } = null!;

    public Guid RevieweeId { get; set; }
    public UserViewModel Reviewee { get; set; } = null!;

    public int Rate { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }
}