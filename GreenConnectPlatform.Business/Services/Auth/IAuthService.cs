using GreenConnectPlatform.Business.Models.Auth;

namespace GreenConnectPlatform.Business.Services.Auth;

public interface IAuthService
{
    Task<AuthResponse> LoginOrRegisterAsync(LoginOrRegisterRequest request);
}