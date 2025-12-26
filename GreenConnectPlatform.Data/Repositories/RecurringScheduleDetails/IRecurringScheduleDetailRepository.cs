using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.RecurringScheduleDetails;

public interface IRecurringScheduleDetailRepository : IBaseRepository<RecurringScheduleDetail, Guid>
{
    Task<RecurringScheduleDetail?> GetByRecurringScheduleIdAsync(Guid id, Guid recurringScheduleId);
}