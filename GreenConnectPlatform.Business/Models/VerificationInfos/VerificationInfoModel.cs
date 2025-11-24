using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class VerificationInfoModel
{
    public Guid UserId { get; set; }
    public UserViewModel User { get; set; } = new();
    public VerificationStatus Status { get; set; }

    public string? DocumentFrontUrl { get; set; }

    public string? DocumentBackUrl { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public Guid? ReviewerId { get; set; }
    public UserViewModel Reviewer { get; set; } = new();

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewerNotes { get; set; }
}