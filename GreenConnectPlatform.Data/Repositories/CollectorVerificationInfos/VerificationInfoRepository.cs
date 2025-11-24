using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
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
        return await _dbSet
            .Include(v => v.Reviewer)
            .Include(v => v.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(v => v.UserId == userId);
    }

    public async Task<(List<CollectorVerificationInfo> Items, int TotalCount)> SearchAsync(bool sortBySubmittedAt, VerificationStatus? status, int pageIndex, int pageSize)
    {
        var query = _dbSet.AsNoTracking();
        if(sortBySubmittedAt)
            query = query.OrderByDescending(v => v.SubmittedAt);
        else
            query = query.OrderBy(v => v.SubmittedAt);
        if (status != null)
            query = query.Where(v => v.Status == status);
        var totalCount = await query.CountAsync();
        var verifications = await query
            .Include(v => v.Reviewer)
            .Include(v => v.User)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (verifications, totalCount);
    }
}