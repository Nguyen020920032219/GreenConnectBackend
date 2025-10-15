namespace GreenConnectPlatform.Data.Entities;

public class Feedback
{
    public Guid FeedbackId { get; set; }

    public Guid TransactionId { get; set; }

    public Guid ReviewerId { get; set; }

    public Guid RevieweeId { get; set; }

    public int Rate { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User Reviewee { get; set; } = null!;

    public virtual User Reviewer { get; set; } = null!;

    public virtual Transaction Transaction { get; set; } = null!;
}