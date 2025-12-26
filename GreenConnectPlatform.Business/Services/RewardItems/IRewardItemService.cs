using GreenConnectPlatform.Business.Models.RewardItems;

namespace GreenConnectPlatform.Business.Services.RewardItems;

public interface IRewardItemService
{
    Task<List<RewardItemModel>> GetAvailableRewardsAsync();
    Task<List<RedemptionHistoryModel>> GetMyRedemptionHistoryAsync(Guid userId);
    Task RedeemRewardAsync(Guid userId, int rewardItemId);
    Task<RewardItemModel> CreateRewardItemAsync(RewardItemCreateModel request);
    Task<RewardItemModel> UpdateRewardItemAsync(int id, RewardItemUpdateModel request);
    Task DeleteRewardItemAsync(int id);
}