using System.ComponentModel.DataAnnotations;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Users;

public class SubmitVerificationRequest
{
    [Required] public BuyerType BuyerType { get; set; }
    [Required] public string DocumentFrontUrl { get; set; } = null!;
    public string? DocumentBackUrl { get; set; }
}