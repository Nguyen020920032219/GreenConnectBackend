using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.AI;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Business.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly IEkycService _ekycService;
    private readonly IFileStorageService _fileStorageService;
    private readonly INotificationService _notificationService;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;
    private readonly IVerificationInfoRepository _verificationInfoRepository;

    public ProfileService(
        UserManager<User> userManager,
        IProfileRepository profileRepository,
        IVerificationInfoRepository verificationInfoRepository,
        IFileStorageService fileStorageService,
        IEkycService ekycService,
        INotificationService notificationService)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _verificationInfoRepository = verificationInfoRepository;
        _fileStorageService = fileStorageService;
        _ekycService = ekycService;
        _notificationService = notificationService;
    }

    public async Task<ProfileModel> GetMyProfileAsync(Guid userId)
    {
        var profile = await _profileRepository.GetByUserIdWithRankAsync(userId);

        if (profile == null || profile.User == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy hồ sơ người dùng.");

        var roles = await _userManager.GetRolesAsync(profile.User);

        var avatarSignedUrl = !string.IsNullOrEmpty(profile.AvatarUrl)
            ? await _fileStorageService.GetReadSignedUrlAsync(profile.AvatarUrl)
            : null;

        return new ProfileModel
        {
            UserId = profile.User.Id,
            ProfileId = profile.ProfileId,
            FullName = profile.User.FullName ?? "",
            PhoneNumber = profile.User.PhoneNumber ?? "",
            AvatarUrl = avatarSignedUrl,
            PointBalance = profile.PointBalance,
            CreditBalance = profile.CreditBalance,
            Rank = profile.Rank?.Name ?? "Bronze",
            Roles = roles,
            Address = profile.Address,
            Gender = profile.Gender,
            DateOfBirth = profile.DateOfBirth,
            BankCode = profile.BankCode,
            BankAccountNumber = profile.BankAccountNumber,
            BankAccountName = profile.BankAccountName
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

        if (request.BankCode != null) profile.BankCode = request.BankCode;
        if (request.BankAccountNumber != null) profile.BankAccountNumber = request.BankAccountNumber;
        if (request.BankAccountName != null) profile.BankAccountName = request.BankAccountName.ToUpper();

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

    public async Task SubmitVerificationAsync(Guid userId, SubmitEkycRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "User not found.");

        // 1. Gọi FPT AI để xác minh
        var ocrResult = await _ekycService.ExtractIdCardInfoAsync(request.FrontImage);

        if (!ocrResult.IsValid)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "EKYC_FAILED", ocrResult.ErrorMessage);

        // 2. Logic tuổi (Tùy chọn: > 18 tuổi)
        if (ocrResult.Dob.HasValue)
        {
            var age = DateTime.UtcNow.Year - ocrResult.Dob.Value.Year;
            if (age < 18)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "AGE_INVALID", "Bạn chưa đủ 18 tuổi.");
        }

        // 3. Tự động duyệt (Auto Approve)
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
        {
            verificationInfo = new CollectorVerificationInfo { UserId = userId };
            await _verificationInfoRepository.AddAsync(verificationInfo);
        }

        // Cập nhật thông tin từ thẻ vào DB
        verificationInfo.IdentityNumber = ocrResult.IdNumber;
        verificationInfo.FullnameOnId = ocrResult.FullName; // Lưu tên thật
        verificationInfo.DateOfBirth = ocrResult.Dob; // Lưu ngày sinh
        verificationInfo.PlaceOfOrigin = ocrResult.Address;
        verificationInfo.IssuedBy = "FPT.AI Verified";

        verificationInfo.Status = VerificationStatus.Approved; // DUYỆT LUÔN
        verificationInfo.SubmittedAt = DateTime.UtcNow;
        verificationInfo.ReviewedAt = DateTime.UtcNow;
        verificationInfo.ReviewerNotes = $"Auto-verified via eKYC. Name: {ocrResult.FullName}";

        await _verificationInfoRepository.UpdateAsync(verificationInfo);

        // 4. Nâng cấp Role
        user.Status = UserStatus.Active;
        user.BuyerType = request.BuyerType;
        await _userManager.UpdateAsync(user);

        if (await _userManager.IsInRoleAsync(user, "Household"))
            await _userManager.RemoveFromRoleAsync(user, "Household");

        var newRole = request.BuyerType == BuyerType.Individual ? "IndividualCollector" : "BusinessCollector";
        if (!await _userManager.IsInRoleAsync(user, newRole))
            await _userManager.AddToRoleAsync(user, newRole);

        // 5. Gửi Noti
        _ = _notificationService.SendNotificationAsync(userId, "Xác minh thành công", "Tài khoản đã được nâng cấp!");
    }
}