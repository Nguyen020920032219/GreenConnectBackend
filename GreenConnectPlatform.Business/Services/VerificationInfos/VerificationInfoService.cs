using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Business.Services.Notifications;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Business.Services.VerificationInfos;

public class VerificationInfoService : IVerificationInfoService
{
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly UserManager<User> _userManager;
    private readonly IVerificationInfoRepository _verificationInfoRepository;

    public VerificationInfoService(
        IVerificationInfoRepository verificationInfoRepository,
        UserManager<User> userManager,
        INotificationService notificationService,
        IMapper mapper)
    {
        _verificationInfoRepository = verificationInfoRepository;
        _userManager = userManager;
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<VerificationInfoOveralModel>> GetVerificationInfos(int pageNumber, int pageSize,
        bool sortBySubmittedAt, VerificationStatus? sortByStatus)
    {
        var (items, totalCount) = await _verificationInfoRepository.SearchAsync(
            sortBySubmittedAt,
            sortByStatus,
            pageNumber,
            pageSize);
        var data = _mapper.Map<List<VerificationInfoOveralModel>>(items);
        return new PaginatedResult<VerificationInfoOveralModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<VerificationInfoModel> GetVerificationInfo(Guid userId)
    {
        // [FIX] Dùng GetByUserIdAsync để đảm bảo có Include User info
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);

        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông tin xác thực");

        return _mapper.Map<VerificationInfoModel>(verificationInfo);
    }

    public async Task VerifyCollector(Guid userId, Guid reviewerId, bool isAccepted, string? reviewerNotes)
    {
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông tin xác thực");

        var user = verificationInfo.User;
        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "User liên kết không tồn tại");

        // Update SecurityStamp để invalidate các token cũ (bảo mật)
        if (string.IsNullOrEmpty(user.SecurityStamp))
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            await _userManager.UpdateSecurityStampAsync(user);
        }

        if (isAccepted)
        {
            verificationInfo.Status = VerificationStatus.Approved;
            user.Status = UserStatus.Active;

            // Xác định Role mới dựa trên BuyerType đã đăng ký
            var newRole = user.BuyerType == BuyerType.Business ? "BusinessCollector" : "IndividualCollector";

            // Xóa Role Household cũ
            if (await _userManager.IsInRoleAsync(user, "Household"))
                await _userManager.RemoveFromRoleAsync(user, "Household");

            // Thêm Role mới
            if (!await _userManager.IsInRoleAsync(user, newRole)) await _userManager.AddToRoleAsync(user, newRole);

            // Noti
            var title = "Hồ sơ đã được duyệt!";
            var body = "Chúc mừng! Tài khoản của bạn đã được nâng cấp thành công. Hãy bắt đầu thu gom ngay.";
            var data = new Dictionary<string, string> { { "type", "Verification" }, { "status", "Approved" } };
            _ = _notificationService.SendNotificationAsync(userId, title, body, data);
        }
        else
        {
            verificationInfo.Status = VerificationStatus.Rejected;
            // Reset BuyerType để user có thể chọn lại loại hình khác nếu muốn
            // user.BuyerType = null; // (Tùy chọn: Có thể giữ lại để họ biết họ từng đăng ký gì)

            await _userManager.UpdateAsync(user);

            // Noti
            var title = "Hồ sơ bị từ chối";
            var body = $"Lý do: {reviewerNotes ?? "Thông tin không hợp lệ"}. Vui lòng cập nhật lại hồ sơ.";
            var data = new Dictionary<string, string> { { "type", "Verification" }, { "status", "Rejected" } };
            _ = _notificationService.SendNotificationAsync(userId, title, body, data);
        }

        verificationInfo.ReviewerId = reviewerId;
        verificationInfo.ReviewedAt = DateTime.Now;
        verificationInfo.ReviewerNotes = reviewerNotes;

        await _verificationInfoRepository.UpdateAsync(verificationInfo);
    }
}