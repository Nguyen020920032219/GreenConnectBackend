using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Users;

public class UserModel
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public int PointBalance { get; set; }
    public string Rank { get; set; } = "Bronze";
    public IList<string> Roles { get; set; } = new List<string>();
    public UserStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}