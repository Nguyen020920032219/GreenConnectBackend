using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.RecurringScheduleDetails;

public class RecurringScheduleDetailRepository : BaseRepository<GreenConnectDbContext, RecurringScheduleDetail, Guid>, IRecurringScheduleDetailRepository
{
    public RecurringScheduleDetailRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<RecurringScheduleDetail?> GetByRecurringScheduleIdAsync(Guid id, Guid recurringScheduleId)
    {
        return await _dbSet
            .Include(s => s.RecurringSchedule)
            .ThenInclude(s => s.Household)
            .FirstOrDefaultAsync(s => s.Id == id && s.RecurringScheduleId == recurringScheduleId);
    }
}