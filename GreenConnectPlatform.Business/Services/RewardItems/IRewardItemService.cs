using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.RewardItems;

namespace GreenConnectPlatform.Business.Services.RewardItems;

public interface IRewardItemService
{
    Task<PaginatedResult<RewardItemModel>> GetRewardItems(int pageIndex, int pageSize, string? name, bool sortByPoint);

    Task<RewardItemModel> GetByRewardItemIdAsync(int id);

    Task<RewardItemModel> CreateRewardItemAsync(RewardItemCreateModel model);

    Task<RewardItemModel> UpdateRewardItemAsync(int id, RewardItemUpdateModel model);

    Task DeleteRewardItemAsync(int id);
}