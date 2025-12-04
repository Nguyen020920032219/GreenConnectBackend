using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.RewardItems;

public class UserRewardRedemptionRepository : BaseRepository<GreenConnectDbContext, UserRewardRedemption, object>,
    IUserRewardRedemptionRepository
{
    public UserRewardRedemptionRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<List<UserRewardRedemption>> GetHistoryByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(x => x.RewardItem)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RedemptionDate)
            .ToListAsync();
    }
}