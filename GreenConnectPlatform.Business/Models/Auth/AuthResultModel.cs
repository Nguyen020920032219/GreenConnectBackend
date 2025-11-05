using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.Auth;

public class AuthResultModel
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public UserModel? User { get; set; }
    public List<string>? Errors { get; set; }
}