using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScrapPosts;

public class ScrapPostRepository : BaseRepository<GreenConnectDbContext, ScrapPost, Guid>, IScrapPostRepository
{
    public ScrapPostRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}