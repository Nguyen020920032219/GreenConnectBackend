using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.UserDevices;

public class UserDeviceRepository : BaseRepository<GreenConnectDbContext, UserDevice, Guid>, IUserDeviceRepository
{
    public UserDeviceRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<UserDevice?> GetByTokenAsync(string fcmToken)
    {
        return await _dbSet.FirstOrDefaultAsync(d => d.FcmToken == fcmToken);
    }

    public async Task<List<string>> GetTokensByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(d => d.UserId == userId)
            .Select(d => d.FcmToken)
            .ToListAsync();
    }
}