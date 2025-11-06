using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class CollectorVerificationInfo
{
    public Guid UserId { get; set; }
    public VerificationStatus Status { get; set; } = VerificationStatus.NotSubmitted;

    public string? DocumentFrontUrl { get; set; }

    public string? DocumentBackUrl { get; set; }

    public DateTime? SubmittedAt { get; set; }
    public Guid? ReviewerId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewerNotes { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual User? Reviewer { get; set; }
}