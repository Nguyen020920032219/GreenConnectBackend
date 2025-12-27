using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.RewardItems;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;
using GreenConnectPlatform.Data.Repositories.PaymentPackages;
using GreenConnectPlatform.Data.Repositories.PointHistories;
using GreenConnectPlatform.Data.Repositories.Profiles;
using GreenConnectPlatform.Data.Repositories.RewardItems;
using GreenConnectPlatform.Data.Repositories.UserPackages;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.RewardItems;

public class RewardItemService : IRewardItemService
{
    private readonly ICreditTransactionHistoryRepository _creditHistoryRepo;

    private readonly IMapper _mapper;
    private readonly IPaymentPackageRepository _paymentPackageRepo;
    private readonly IPointHistoryRepository _pointHistoryRepo;
    private readonly IProfileRepository _profileRepo;
    private readonly IUserRewardRedemptionRepository _redemptionRepo;
    private readonly IRewardItemRepository _rewardRepo;

    private readonly IUserPackageRepository _userPackageRepo;

    public RewardItemService(
        IRewardItemRepository rewardRepo,
        IUserRewardRedemptionRepository redemptionRepo,
        IProfileRepository profileRepo,
        IPointHistoryRepository pointHistoryRepo,
        IUserPackageRepository userPackageRepo,
        IPaymentPackageRepository paymentPackageRepo,
        ICreditTransactionHistoryRepository creditHistoryRepo,
        IMapper mapper)
    {
        _rewardRepo = rewardRepo;
        _redemptionRepo = redemptionRepo;
        _profileRepo = profileRepo;
        _pointHistoryRepo = pointHistoryRepo;
        _userPackageRepo = userPackageRepo;
        _paymentPackageRepo = paymentPackageRepo;
        _creditHistoryRepo = creditHistoryRepo;
        _mapper = mapper;
    }

    public async Task<List<RewardItemModel>> GetAvailableRewardsAsync()
    {
        var items = await _rewardRepo.GetAllAsync();
        return _mapper.Map<List<RewardItemModel>>(items);
    }

    public async Task<List<RedemptionHistoryModel>> GetMyRedemptionHistoryAsync(Guid userId)
    {
        var history = await _redemptionRepo.GetHistoryByUserIdAsync(userId);

        return history.Select(h => new RedemptionHistoryModel
        {
            RewardItemId = h.RewardItemId,
            ItemName = h.RewardItem?.ItemName ?? "Unknown Reward",
            Description = h.RewardItem?.Description,
            PointsSpent = h.RewardItem?.PointsCost ?? 0,
            ImageUrl = h.RewardItem?.ImageUrl,
            RedemptionDate = h.RedemptionDate
        }).ToList();
    }

    public async Task RedeemRewardAsync(Guid userId, int rewardItemId)
    {
        // 1. Validate Quà
        var reward = await _rewardRepo.GetByIdAsync(rewardItemId);
        if (reward == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Món quà không tồn tại.");

        // 2. Validate User & Điểm
        var profile = await _profileRepo.GetByUserIdWithRankAsync(userId);
        if (profile == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy hồ sơ người dùng.");

        if (profile.PointBalance < reward.PointsCost)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                $"Bạn không đủ điểm. Cần {reward.PointsCost} điểm, bạn chỉ có {profile.PointBalance} điểm.");

        // 3. Trừ điểm (Gamification)
        profile.PointBalance -= reward.PointsCost;

        // 4. Xử lý Trao Quà (Grant Reward)
        if (reward.Type == "Credit")
        {
            if (int.TryParse(reward.Value, out var creditAmount))
            {
                // Cộng Credit vào Profile
                profile.CreditBalance += creditAmount;

                // Ghi log Credit
                var creditLog = new CreditTransactionHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Amount = creditAmount,
                    BalanceAfter = profile.CreditBalance,
                    Type = "Redemption", // Loại giao dịch: Đổi thưởng
                    ReferenceId = null, // Hoặc lưu ID redemption nếu cần
                    Description = $"Đổi quà: {reward.ItemName}",
                    CreatedAt = DateTime.Now
                };
                await _creditHistoryRepo.AddAsync(creditLog);
            }
        }
        else if (reward.Type == "Package")
        {
            // Value format: "PackageId|Days"
            var parts = reward.Value.Split('|');
            if (parts.Length > 0 && Guid.TryParse(parts[0], out var packageId))
            {
                var days = parts.Length > 1 && int.TryParse(parts[1], out var d) ? d : 1;
                await ActivatePackageReward(userId, packageId, days);
            }
        }

        // 5. Lưu thay đổi Profile (Điểm & Credit)
        await _profileRepo.UpdateAsync(profile);

        // 6. Ghi lịch sử Đổi Quà
        var redemption = new UserRewardRedemption
        {
            UserId = userId,
            RewardItemId = rewardItemId,
            RedemptionDate = DateTime.Now
        };
        await _redemptionRepo.AddAsync(redemption);

        // 7. Ghi lịch sử Trừ Điểm (Point History)
        var pointHistory = new PointHistory
        {
            PointHistoryId = Guid.NewGuid(),
            UserId = userId,
            PointChange = -reward.PointsCost,
            Reason = $"Đổi quà: {reward.ItemName}",
            CreatedAt = DateTime.Now
        };
        await _pointHistoryRepo.AddAsync(pointHistory);
    }

    public async Task<RewardItemModel> CreateRewardItemAsync(RewardItemCreateModel request)
    {
        var item = _mapper.Map<RewardItem>(request);
        // ID tự tăng nên không cần gán

        await _rewardRepo.AddAsync(item);
        return _mapper.Map<RewardItemModel>(item);
    }

    public async Task<RewardItemModel> UpdateRewardItemAsync(int id, RewardItemUpdateModel request)
    {
        var item = await _rewardRepo.GetByIdAsync(id);
        if (item == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Món quà không tồn tại.");

        // Map dữ liệu từ request sang item (chỉ map những field khác null nếu config AutoMapper, hoặc map tay)
        if (request.ItemName != null) item.ItemName = request.ItemName;
        if (request.Description != null) item.Description = request.Description;
        if (request.PointsCost.HasValue) item.PointsCost = request.PointsCost.Value;
        if (request.ImageUrl != null) item.ImageUrl = request.ImageUrl;
        if (request.Type != null) item.Type = request.Type;
        if (request.Value != null) item.Value = request.Value;

        await _rewardRepo.UpdateAsync(item);
        return _mapper.Map<RewardItemModel>(item);
    }

    public async Task DeleteRewardItemAsync(int id)
    {
        var item = await _rewardRepo.GetByIdAsync(id);
        if (item == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Món quà không tồn tại.");

        // [Optional] Check xem quà đã có người đổi chưa? Nếu có rồi thì chặn xóa hoặc soft delete.
        // Ở mức độ MVP, ta cho phép xóa (User đã đổi rồi thì lịch sử vẫn còn trong bảng Redemption, chỉ mất thông tin quà gốc).

        await _rewardRepo.DeleteAsync(item);
    }

    private async Task ActivatePackageReward(Guid userId, Guid packageId, int days)
    {
        var package = await _paymentPackageRepo.GetByIdAsync(packageId);
        if (package == null) return;

        var userPackages = await _userPackageRepo.FindAsync(up => up.UserId == userId);
        var currentPackage = userPackages.FirstOrDefault();

        if (currentPackage == null)
        {
            // Tạo gói mới
            currentPackage = new UserPackage
            {
                UserPackageId = Guid.NewGuid(),
                UserId = userId,
                PackageId = packageId,
                ActivationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(days),
                RemainingConnections = package.ConnectionAmount
            };
            await _userPackageRepo.AddAsync(currentPackage);
        }
        else
        {
            // Nâng cấp/Gia hạn gói cũ
            currentPackage.PackageId = packageId;

            // Nếu gói cũ còn hạn thì cộng thêm ngày, nếu hết thì tính từ nay
            var baseDate = currentPackage.ExpirationDate > DateTime.Now
                ? currentPackage.ExpirationDate.Value
                : DateTime.Now;

            currentPackage.ExpirationDate = baseDate.AddDays(days);

            // Reset hoặc cộng dồn connections tùy logic (ở đây là reset theo gói mới)
            if (package.ConnectionAmount.HasValue)
                // Có thể cộng dồn: (currentPackage.RemainingConnections ?? 0) + package.ConnectionAmount
                currentPackage.RemainingConnections = package.ConnectionAmount;

            await _userPackageRepo.UpdateAsync(currentPackage);
        }
    }
}