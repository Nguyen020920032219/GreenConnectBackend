using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.ScrapPostTimeSlots;

public class ScrapPostTimeSlotRepository : BaseRepository<GreenConnectDbContext, ScrapPostTimeSlot, Guid>,
    IScrapPostTimeSlotRepository
{
    public ScrapPostTimeSlotRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<ScrapPostTimeSlot?> GetSlotTimeByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Id == id);
    }
}