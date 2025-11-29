using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.PointHistories;

public class PointHistoryRepository : BaseRepository<GreenConnectDbContext, PointHistory, Guid>, IPointHistoryRepository
{
    public PointHistoryRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<(List<PointHistory> Items, int TotalCount)> GetPagedPointHistoriesAsync(Guid? userId, Guid currentUserId, bool sortByCreateAtDesc, int pageIndex, int pageSize)
    {
        var query  = _dbSet
            .Include(x => x.User)
            .ThenInclude(u => u.Profile)
            .AsQueryable();
        if (userId != null)
        {
            query = query.Where(x => x.UserId == userId);
        }
        else
        {
            query = query.Where(x => x.UserId == currentUserId);
        }
        if(sortByCreateAtDesc)
            query = query.OrderByDescending(x => x.CreatedAt);
        else
            query = query.OrderBy(x => x.CreatedAt);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }
}