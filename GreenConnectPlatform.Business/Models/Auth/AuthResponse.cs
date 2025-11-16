using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Models.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; } = null!;
    public UserViewModel User { get; set; } = null!;
}