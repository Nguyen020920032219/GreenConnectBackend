using GreenConnectPlatform.Business.Models.Auth;

namespace GreenConnectPlatform.Business.Services.Auth;

public interface IAuthService
{
    Task<(AuthResponse Response, bool IsNewUser)> LoginOrRegisterAsync(LoginOrRegisterRequest request);

    Task<AuthResponse> AdminLoginAsync(AdminLoginRequest request);
}