namespace GreenConnectPlatform.Business.Models.Users;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public int PointBalance { get; set; }
    public int CreditBalance { get; set; }
    public string Rank { get; set; } = "Bronze";
    public IList<string> Roles { get; set; } = new List<string>();
    public string? AvatarUrl { get; set; }
}