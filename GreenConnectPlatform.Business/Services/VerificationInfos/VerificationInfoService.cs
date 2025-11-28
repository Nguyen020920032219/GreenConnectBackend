using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GreenConnectPlatform.Business.Services.VerificationInfos;

public class VerificationInfoService : IVerificationInfoService
{
    private readonly GreenConnectDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IVerificationInfoRepository _verificationInfoRepository;

    public VerificationInfoService(IVerificationInfoRepository verificationInfoRepository,
        GreenConnectDbContext context,
        UserManager<User> userManager,
        IMapper mapper)
    {
        _verificationInfoRepository = verificationInfoRepository;
        _context = context;
        _userManager = userManager;
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
        var verificationInfo = await _verificationInfoRepository.GetByIdAsync(userId);
        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông tin xác thực");
        return _mapper.Map<VerificationInfoModel>(verificationInfo);
    }

    public async Task<VerificationInfoModel> GetMyVerificationInfo(Guid userId)
    {
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Bạn không có thông tin xác thực nào");
        return _mapper.Map<VerificationInfoModel>(verificationInfo);
    }

    public async Task VerifyCollector(Guid userId, Guid reviewerId, bool isAccepted, string? reviewerNotes)
    {
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông tin xác thực");
        var user = verificationInfo.User;
        if (string.IsNullOrEmpty(user.SecurityStamp))
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            await _userManager.UpdateSecurityStampAsync(user);
        }

        if (isAccepted)
        {
            verificationInfo.Status = VerificationStatus.Approved;
            verificationInfo.User.Status = UserStatus.Active;
            var newRole = user.BuyerType == BuyerType.Business ? "BusinessCollector" : "IndividualCollector";

            var roles = await _userManager.GetRolesAsync(user);
            var roleToMoves = roles.Where(r => r == "Household").ToList();
            if (roleToMoves.Any())
                await _userManager.RemoveFromRolesAsync(user, roleToMoves);
            if (!await _userManager.IsInRoleAsync(user, newRole))
                await _userManager.AddToRoleAsync(user, newRole);
        }
        else
        {
            verificationInfo.Status = VerificationStatus.Rejected;
            user.BuyerType = null;
            await _userManager.UpdateAsync(user);
        }

        verificationInfo.ReviewerId = reviewerId;
        verificationInfo.ReviewedAt = DateTime.UtcNow;
        verificationInfo.ReviewerNotes = reviewerNotes;
        await _verificationInfoRepository.UpdateAsync(verificationInfo);
    }

    public async Task<VerificationInfoModel> UpdateVerificationInfo(Guid userId, BuyerType? buyerType,
        string? documentFrontUrl, string? documentBackUrl)
    {
        var verificationInfo = await _verificationInfoRepository.GetByUserIdAsync(userId);
        if (verificationInfo == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy thông tin xác thực");
        if (verificationInfo.Status == VerificationStatus.Approved)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể cập nhật thông tin xác thực đã được phê duyệt");
        if (verificationInfo.UserId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền cập nhật thông tin xác thực này");
        if (!string.IsNullOrEmpty(documentFrontUrl))
            verificationInfo.DocumentFrontUrl = documentFrontUrl;
        if (!string.IsNullOrEmpty(documentBackUrl))
            verificationInfo.DocumentBackUrl = documentBackUrl;
        if (buyerType.HasValue)
            verificationInfo.User.BuyerType = buyerType;
        verificationInfo.SubmittedAt = DateTime.UtcNow;
        await _verificationInfoRepository.UpdateAsync(verificationInfo);
        return _mapper.Map<VerificationInfoModel>(verificationInfo);
    }
}