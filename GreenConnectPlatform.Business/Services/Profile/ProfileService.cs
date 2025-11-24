using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Business.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;
    private readonly IVerificationInfoRepository _verificationInfoRepository;

    public ProfileService(
        UserManager<User> userManager,
        IProfileRepository profileRepository,
        IVerificationInfoRepository verificationInfoRepository,
        IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _verificationInfoRepository = verificationInfoRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<ProfileModel> GetMyProfileAsync(Guid userId)
    {
        var profile = await _profileRepository.GetByUserIdWithRankAsync(userId);

        if (profile == null || profile.User == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy hồ sơ người dùng.");

        var roles = await _userManager.GetRolesAsync(profile.User);

        return new ProfileModel
        {
            UserId = profile.User.Id,
            ProfileId = profile.ProfileId,
            FullName = profile.User.FullName ?? "",
            PhoneNumber = profile.User.PhoneNumber ?? "",
            AvatarUrl = profile.AvatarUrl,
            PointBalance = profile.PointBalance,
            Rank = profile.Rank?.Name ?? "Bronze",
            Roles = roles,
            Address = profile.Address,
            Gender = profile.Gender,
            DateOfBirth = profile.DateOfBirth
        };
    }

    public async Task<ProfileModel> UpdateMyProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy người dùng.");

        if (!string.IsNullOrEmpty(request.FullName) && user.FullName != request.FullName)
        {
            user.FullName = request.FullName;
            await _userManager.UpdateAsync(user);
        }

        var profile = await _profileRepository.GetByUserIdWithRankAsync(userId);

        if (profile == null)
        {
            profile = new Data.Entities.Profile { UserId = userId, ProfileId = Guid.NewGuid() };
            await _profileRepository.AddAsync(profile);
        }

        if (!string.IsNullOrEmpty(request.Address)) profile.Address = request.Address;
        if (request.Gender.HasValue) profile.Gender = request.Gender;
        if (request.DateOfBirth.HasValue) profile.DateOfBirth = request.DateOfBirth;

        await _profileRepository.UpdateAsync(profile);

        return await GetMyProfileAsync(userId);
    }

    public async Task UpdateAvatarAsync(Guid userId, UpdateFileRequestModel request)
    {
        var profile = await _profileRepository.GetByUserIdWithRankAsync(userId);

        if (profile == null)
        {
            profile = new Data.Entities.Profile { UserId = userId, ProfileId = Guid.NewGuid() };
            await _profileRepository.AddAsync(profile);
        }

        var oldAvatarUrl = profile.AvatarUrl;
        profile.AvatarUrl = request.FileName;

        await _profileRepository.UpdateAsync(profile);

        if (!string.IsNullOrEmpty(oldAvatarUrl) && oldAvatarUrl != request.FileName)
            await _fileStorageService.DeleteFileAsync(oldAvatarUrl);
    }

    public async Task SubmitVerificationAsync(Guid userId, SubmitVerificationRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy người dùng.");

        user.Status = UserStatus.PendingVerification;
        user.BuyerType = request.BuyerType;
        await _userManager.UpdateAsync(user);

        if (await _userManager.IsInRoleAsync(user, "Household"))
            await _userManager.RemoveFromRoleAsync(user, "Household");

        var newRole = request.BuyerType == BuyerType.Individual ? "IndividualCollector" : "BusinessCollector";
        if (!await _userManager.IsInRoleAsync(user, newRole)) await _userManager.AddToRoleAsync(user, newRole);

        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);

        string? oldFront = null;
        string? oldBack = null;

        if (verificationInfo == null)
        {
            verificationInfo = new CollectorVerificationInfo
            {
                UserId = userId,
                Status = VerificationStatus.PendingReview,
                DocumentFrontUrl = request.DocumentFrontUrl,
                DocumentBackUrl = request.DocumentBackUrl,
                SubmittedAt = DateTime.UtcNow
            };
            await _verificationInfoRepository.AddAsync(verificationInfo);
        }
        else
        {
            oldFront = verificationInfo.DocumentFrontUrl;
            oldBack = verificationInfo.DocumentBackUrl;

            verificationInfo.Status = VerificationStatus.PendingReview;
            verificationInfo.DocumentFrontUrl = request.DocumentFrontUrl;
            verificationInfo.DocumentBackUrl = request.DocumentBackUrl;
            verificationInfo.SubmittedAt = DateTime.UtcNow;
            verificationInfo.ReviewerId = null;
            verificationInfo.ReviewerNotes = null;

            await _verificationInfoRepository.UpdateAsync(verificationInfo);
        }

        if (!string.IsNullOrEmpty(oldFront) && oldFront != request.DocumentFrontUrl)
            await _fileStorageService.DeleteFileAsync(oldFront);
        if (!string.IsNullOrEmpty(oldBack) && oldBack != request.DocumentBackUrl)
            await _fileStorageService.DeleteFileAsync(oldBack);
    }
}