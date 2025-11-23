using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;

public class VerificationInfoRepository : BaseRepository<GreenConnectDbContext, CollectorVerificationInfo, Guid>,
    IVerificationInfoRepository
{
    public VerificationInfoRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<CollectorVerificationInfo?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(v => v.UserId == userId);
    }
}