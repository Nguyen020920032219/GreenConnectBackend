using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScrapPostTimeSlots;

public interface IScrapPostTimeSlotRepository : IBaseRepository<ScrapPostTimeSlot, Guid>
{
    Task<ScrapPostTimeSlot?> GetSlotTimeByIdAsync(Guid id);
}