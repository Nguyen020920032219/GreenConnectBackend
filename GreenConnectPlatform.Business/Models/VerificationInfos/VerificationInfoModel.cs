using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class VerificationInfoModel
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
    public DateTime? ReviewedAt { get; set; }
    public string? ReviewerNotes { get; set; }
}