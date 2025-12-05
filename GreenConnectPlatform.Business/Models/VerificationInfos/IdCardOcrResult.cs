namespace GreenConnectPlatform.Business.Models.VerificationInfos;

public class IdCardOcrResult
{
    public bool IsValid { get; set; }
    public string IdNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Address { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}