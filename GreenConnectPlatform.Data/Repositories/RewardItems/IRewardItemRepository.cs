using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.RewardItems;

public interface IRewardItemRepository : IBaseRepository<RewardItem, int>
{
    Task<RewardItem?> GetByRewardItemIdAsync(int id);
    Task<(List<RewardItem> Items, int TotalCount)> GetRewardItemsAsync(int pageIndex, int pageSize, string? name, bool sortByPoint);
    
    Task<int> GetTotalRewardItemsMaxAsync();

    Task<List<RewardItem>> GetRewardItemsForReport();
}