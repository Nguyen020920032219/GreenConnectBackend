using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.RecurringSchedules;

public class RecurringScheduleRepository : BaseRepository<GreenConnectDbContext, RecurringSchedule, Guid>,
    IRecurringScheduleRepository
{
    public RecurringScheduleRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<RecurringSchedule> Items, int TotalCount)> GetPagedRecurringSchedulesAsync(Guid userId,
        int pageNumber,
        int pageSize, bool sortByCreatedAt)
    {
        var query = _dbSet.Where(q => q.HouseholdId == userId).AsNoTracking();
        if (sortByCreatedAt)
            query = query.OrderByDescending(q => q.CreatedAt);
        else
            query = query.OrderBy(q => q.CreatedAt);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(q => q.ScheduleDetails)
            .ThenInclude(s => s.ScrapCategory)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<RecurringSchedule?> GetRecurringScheduleByIdAsync(Guid id)
    {
        return await _dbSet.Include(q => q.ScheduleDetails)
            .ThenInclude(s => s.ScrapCategory)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}