using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class CollectorVerificationInfo
{
    public Guid UserId { get; set; }
    public VerificationStatus Status { get; set; }
    public string? IdentityNumber { get; set; }
    public string? FullnameOnId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? PlaceOfOrigin { get; set; }
    public string? IssuedBy { get; set; }
    public DateTime? IssuedDate { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public Guid? ReviewerId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewerNotes { get; set; }

    public virtual User? User { get; set; }
    public virtual User? Reviewer { get; set; }
}