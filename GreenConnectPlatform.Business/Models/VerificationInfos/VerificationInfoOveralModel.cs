using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class VerificationInfoOveralModel
{
    public Guid UserId { get; set; }
    public UserViewModel User { get; set; } = new();
    public VerificationStatus Status { get; set; }

    public string? DocumentFrontUrl { get; set; }

    public string? DocumentBackUrl { get; set; }

    public DateTime? SubmittedAt { get; set; }
}