using GreenConnectPlatform.Bussiness.Models.Users;

namespace GreenConnectPlatform.Bussiness.Services.Auth;

public class AuthResultModel
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public UserModel? User { get; set; }
    public List<string>? Errors { get; set; }
}