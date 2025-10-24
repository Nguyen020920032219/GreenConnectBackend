using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScrapCategories;

public class ScrapCategoryRepository : BaseRepository<GreenConnectDbContext, ScrapCategory, int>,
    IScrapCategoryRepository
{
    public ScrapCategoryRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}