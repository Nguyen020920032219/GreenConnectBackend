using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScrapPosts.ScrapPostDetails;

public class ScrapPostDetailRepository : BaseRepository<GreenConnectDbContext, ScrapPostDetail, Guid>,
    IScrapPostDetailRepository
{
    public ScrapPostDetailRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}