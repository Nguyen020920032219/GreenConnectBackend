using GreenConnectPlatform.Business.Models.Auth;

namespace GreenConnectPlatform.Business.Services.Auth;

public interface IAuthService
{
    Task<AuthResultModel> LoginWithFirebaseAsync(FirebaseLoginRequestModel request);
    Task<AuthResultModel> AdminLoginAsync(AdminLoginRequestModel request);
}