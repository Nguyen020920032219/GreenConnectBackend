using GreenConnectPlatform.Bussiness.Models.Auth;

namespace GreenConnectPlatform.Bussiness.Services.Auth;

public interface IAuthService
{
    Task<AuthResultModel> LoginWithFirebaseAsync(FirebaseLoginRequestModel request);
    Task<AuthResultModel> AdminLoginAsync(AdminLoginRequestModel request);
}