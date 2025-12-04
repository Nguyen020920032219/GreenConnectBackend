using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.RewardItems;

public interface IUserRewardRedemptionRepository : IBaseRepository<UserRewardRedemption, object>
{
    Task<List<UserRewardRedemption>> GetHistoryByUserIdAsync(Guid userId);
}