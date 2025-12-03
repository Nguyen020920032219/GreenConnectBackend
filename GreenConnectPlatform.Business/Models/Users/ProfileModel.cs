using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Users;

public class ProfileModel
{
    public Guid ProfileId { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public int PointBalance { get; set; }
    public int CreditBalance { get; set; }
    public string Rank { get; set; } = "Bronze";
    public IList<string> Roles { get; set; } = new List<string>();
    public string? AvatarUrl { get; set; }
    public string? BankCode { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
}