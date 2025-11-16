using GreenConnectPlatform.Business.Models.Users;

namespace GreenConnectPlatform.Business.Services.Profile;

public interface IProfileService
{
    Task<ProfileModel> GetMyProfileAsync(Guid userId);
    Task<ProfileModel> UpdateMyProfileAsync(Guid userId, UpdateProfileRequest request);
    Task SubmitVerificationAsync(Guid userId, SubmitVerificationRequest request);
}