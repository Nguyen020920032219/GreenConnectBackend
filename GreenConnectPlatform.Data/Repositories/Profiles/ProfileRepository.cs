using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Profiles;

public class ProfileRepository : BaseRepository<GreenConnectDbContext, Profile, Guid>, IProfileRepository
{
    public ProfileRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<Profile?> GetByUserIdWithRankAsync(Guid userId)
    {
        return await _dbSet
            .Include(p => p.Rank)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }
}