using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Profiles;

public class ProfileRepository : BaseRepository<GreenConnectDbContext, Profile, Guid>, IProfileRepository
{
    public ProfileRepository(GreenConnectDbContext context) : base(context)
    {
    }
}