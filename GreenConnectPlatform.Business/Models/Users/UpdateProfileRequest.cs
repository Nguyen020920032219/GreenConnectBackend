using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Users;

public class UpdateProfileRequest
{
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public Gender? Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public LocationModel? Location { get; set; }
    public string? BankCode { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankAccountName { get; set; }
}