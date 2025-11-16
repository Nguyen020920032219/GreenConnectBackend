using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;
    private readonly IVerificationInfoRepository _verificationInfoRepository;

    public ProfileService(UserManager<User> userManager, IProfileRepository profileRepository,
        IVerificationInfoRepository verificationInfoRepository)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _verificationInfoRepository = verificationInfoRepository;
    }

    public async Task<ProfileModel> GetMyProfileAsync(Guid userId)
    {
        var user = await _userManager.Users
            .Include(u => u.Profile)
            .ThenInclude(p => p!.Rank)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new KeyNotFoundException("User not found.");

        var roles = await _userManager.GetRolesAsync(user);

        return new ProfileModel
        {
            UserId = user.Id,
            ProfileId = user.Profile.ProfileId,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.Profile?.AvatarUrl,
            PointBalance = user.Profile?.PointBalance ?? 0,
            Rank = user.Profile?.Rank?.Name ?? "Bronze",
            Roles = roles,
            Address = user.Profile?.Address,
            Gender = user.Profile?.Gender,
            DateOfBirth = user.Profile?.DateOfBirth
        };
    }

    public async Task<ProfileModel> UpdateMyProfileAsync(Guid userId, UpdateProfileRequest request)
    {
        var user = await _userManager.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) throw new KeyNotFoundException("User not found.");

        user.FullName = request.FullName ?? user.FullName;

        if (user.Profile != null)
        {
            user.Profile.Address = request.Address ?? user.Profile.Address;
            user.Profile.Gender = request.Gender ?? user.Profile.Gender;
            user.Profile.DateOfBirth = request.DateOfBirth ?? user.Profile.DateOfBirth;
        }

        await _userManager.UpdateAsync(user);

        return await GetMyProfileAsync(userId);
    }

    public async Task SubmitVerificationAsync(Guid userId, SubmitVerificationRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) throw new KeyNotFoundException("User not found.");

        user.Status = UserStatus.PendingVerification;
        user.BuyerType = request.BuyerType;
        await _userManager.UpdateAsync(user);

        await _userManager.RemoveFromRoleAsync(user, "Household");
        if (request.BuyerType == BuyerType.Individual)
            await _userManager.AddToRoleAsync(user, "IndividualCollector");
        else
            await _userManager.AddToRoleAsync(user, "BusinessCollector");


        var verificationInfo = await _verificationInfoRepository.DbSet()
            .FirstOrDefaultAsync(v => v.UserId == userId);

        if (verificationInfo == null)
        {
            verificationInfo = new CollectorVerificationInfo
            {
                UserId = userId
            };
            await _verificationInfoRepository.DbSet().AddAsync(verificationInfo);
        }

        verificationInfo.Status = VerificationStatus.PendingReview;
        verificationInfo.DocumentFrontUrl = request.DocumentFrontUrl;
        verificationInfo.DocumentBackUrl = request.DocumentBackUrl;
        verificationInfo.SubmittedAt = DateTime.UtcNow;

        await _verificationInfoRepository.DbContext().SaveChangesAsync();
    }
}