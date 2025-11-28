using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.UserPackages;

public class UserPackageRepository : BaseRepository<GreenConnectDbContext, UserPackage, Guid>, IUserPackageRepository
{
    public UserPackageRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}