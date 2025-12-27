using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.RecurringSchedules;

public interface IRecurringScheduleRepository : IBaseRepository<RecurringSchedule, Guid>
{
    Task<(List<RecurringSchedule>Items, int TotalCount)> GetPagedRecurringSchedulesAsync(Guid userId, int pageNumber,
        int pageSize,
        bool sortByCreatedAt);

    Task<RecurringSchedule?> GetRecurringScheduleByIdAsync(Guid id);
}