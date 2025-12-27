using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.AI;
using GreenConnectPlatform.Business.Services.Banks;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;
using GreenConnectPlatform.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Business.Services.Profile;

public class ProfileService : IProfileService
{
    private const int WELCOME_BONUS_CREDIT = 50;
    private readonly IBankService _bankService;
    private readonly ICreditTransactionHistoryRepository _creditHistoryRepository;
    private readonly IEkycService _ekycService;
    private readonly IFileStorageService _fileStorageService;
    private readonly GeometryFactory _geometryFactory;
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
        INotificationService notificationService,
        IBankService bankService,
        ICreditTransactionHistoryRepository creditHistoryRepository)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _verificationInfoRepository = verificationInfoRepository;
        _fileStorageService = fileStorageService;
        _ekycService = ekycService;
        _notificationService = notificationService;
        _bankService = bankService;
        _creditHistoryRepository = creditHistoryRepository;
        _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
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

        string? bankName = null;
        if (!string.IsNullOrEmpty(profile.BankCode))
        {
            var banks = await _bankService.GetSupportedBanksAsync();
            var bank = banks.FirstOrDefault(b => b.Bin == profile.BankCode);
            if (bank != null) bankName = !string.IsNullOrEmpty(bank.ShortName) ? bank.ShortName : bank.Name;
        }

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
            BankName = bankName,
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
            profile = new Data.Entities.Profile
                { UserId = userId, ProfileId = Guid.NewGuid(), RankId = 1, PointBalance = 0 };
            await _profileRepository.AddAsync(profile);
        }

        if (!string.IsNullOrEmpty(request.Address))
        {
            profile.Address = request.Address;
            if (request.Location != null && request.Location.Latitude.HasValue && request.Location.Longitude.HasValue)
                profile.Location = _geometryFactory.CreatePoint(new Coordinate(
                    request.Location.Longitude.Value, request.Location.Latitude.Value));
        }

        if (request.Gender.HasValue) profile.Gender = request.Gender;

        if (request.DateOfBirth.HasValue) profile.DateOfBirth = request.DateOfBirth.Value;

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
            profile = new Data.Entities.Profile
                { UserId = userId, ProfileId = Guid.NewGuid(), RankId = 1, PointBalance = 0 };
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
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy người dùng.");

        var ocrResult = await _ekycService.ExtractIdCardInfoAsync(request.FrontImage);

        if (!ocrResult.IsValid)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "EKYC_FAILED", ocrResult.ErrorMessage);

        if (ocrResult.Dob.HasValue)
        {
            var age = DateTime.UtcNow.Year - ocrResult.Dob.Value.Year;
            if (age < 18)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "AGE_INVALID",
                    "Bạn phải từ 18 tuổi trở lên.");
        }

        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
        {
            verificationInfo = new CollectorVerificationInfo { UserId = userId };
            await _verificationInfoRepository.AddAsync(verificationInfo);
        }

        verificationInfo.IdentityNumber = ocrResult.IdNumber;
        verificationInfo.FullnameOnId = ocrResult.FullName;

        if (ocrResult.Dob.HasValue)
        {
            var dob = ocrResult.Dob.Value;
            verificationInfo.DateOfBirth = dob.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(dob, DateTimeKind.Utc)
                : dob.ToUniversalTime();
        }
        else
        {
            verificationInfo.DateOfBirth = null;
        }

        verificationInfo.PlaceOfOrigin = ocrResult.Address;
        verificationInfo.IssuedBy = "FPT.AI Verified";
        verificationInfo.Status = VerificationStatus.Approved;
        verificationInfo.SubmittedAt = DateTime.UtcNow;
        verificationInfo.ReviewedAt = DateTime.UtcNow;
        verificationInfo.ReviewerNotes = $"Tự động xác minh qua eKYC. Tên: {ocrResult.FullName}";

        var currentRoles = await _userManager.GetRolesAsync(user);
        var isAlreadyCollector = currentRoles.Any(r => r == "IndividualCollector" || r == "BusinessCollector");

        user.Status = UserStatus.Active;
        user.BuyerType = request.BuyerType;
        await _userManager.UpdateAsync(user);

        var rolesToRemove = currentRoles.Where(r => r != "Admin").ToList();
        if (rolesToRemove.Any()) await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

        var newRole = request.BuyerType == BuyerType.Individual ? "IndividualCollector" : "BusinessCollector";
        await _userManager.AddToRoleAsync(user, newRole);

        var notificationMessage = "Tài khoản của bạn đã được nâng cấp thành công!";

        try
        {
            await _verificationInfoRepository.UpdateAsync(verificationInfo);

            if (!isAlreadyCollector)
            {
                var profile = await _profileRepository.GetByUserIdWithRankAsync(userId);
                if (profile == null)
                {
                    profile = new Data.Entities.Profile
                        { UserId = userId, ProfileId = Guid.NewGuid(), RankId = 1, PointBalance = 0 };
                    await _profileRepository.AddAsync(profile);
                }

                profile.CreditBalance += WELCOME_BONUS_CREDIT;
                await _profileRepository.UpdateAsync(profile);

                var creditHistory = new CreditTransactionHistory
                {
                    UserId = userId,
                    Amount = WELCOME_BONUS_CREDIT,
                    BalanceAfter = profile.CreditBalance,
                    Type = "Bonus",
                    Description = "Quà tặng trải nghiệm: Chào mừng bạn gia nhập cộng đồng Collector!",
                    CreatedAt = DateTime.UtcNow
                };
                await _creditHistoryRepository.AddAsync(creditHistory);

                notificationMessage =
                    $"Chúc mừng! Bạn đã trở thành Collector và được tặng {WELCOME_BONUS_CREDIT} điểm trải nghiệm miễn phí.";
            }
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException != null &&
                ex.InnerException.Message.Contains("IX_CollectorVerificationInfos_IdentityNumber"))
                throw new ApiExceptionModel(StatusCodes.Status409Conflict, "DUPLICATE_ID",
                    "Số CCCD/CMND này đã được sử dụng bởi một tài khoản khác.");

            throw;
        }

        _ = _notificationService.SendNotificationAsync(userId, "Xác minh thành công", notificationMessage);
    }
}