using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.RewardItems;

public class RewardItemRepository : BaseRepository<GreenConnectDbContext, RewardItem, int>, IRewardItemRepository
{
    public RewardItemRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<RewardItem?> GetByRewardItemIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(r => r.RewardItemId == id);
    }

    public async Task<(List<RewardItem> Items, int TotalCount)> GetRewardItemsAsync(int pageIndex, int pageSize, string? name, bool sortByPoint)
    {
        var query = _dbSet.AsNoTracking();
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(r => r.ItemName.ToLower().Contains(name.ToLower()));
        }

        if (sortByPoint)
        {
            query = query.OrderByDescending(r => r.PointsCost);
        }
        else
        {
            query = query.OrderBy(r => r.PointsCost);
        }
        var totalCount = await query.CountAsync();
        var rewardItems = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (rewardItems, totalCount);
    }

    public async Task<int> GetTotalRewardItemsMaxAsync()
    {
        return await _dbSet.MaxAsync(r => r.RewardItemId);
    }

    public async Task<List<RewardItem>> GetRewardItemsForReport()
    {
        return await _dbSet.ToListAsync();
    }
}