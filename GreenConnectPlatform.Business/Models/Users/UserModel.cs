namespace GreenConnectPlatform.Business.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = new();
    public string? AvatarUrl { get; set; }
}