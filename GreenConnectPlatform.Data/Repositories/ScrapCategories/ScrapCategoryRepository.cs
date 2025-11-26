using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.ScrapCategories;

public class ScrapCategoryRepository : BaseRepository<GreenConnectDbContext, ScrapCategory, int>,
    IScrapCategoryRepository
{
    public ScrapCategoryRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(IList<ScrapCategory> Items, int TotalCount)> SearchAndPaginateAsync(string? keyword,
        int pageIndex, int pageSize)
    {
        var query = _dbSet.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var term = keyword.Trim().ToLower();
            query = query.Where(x => x.CategoryName.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.CategoryName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> GetCategoryIdMax()
    {
        return await _dbSet.MaxAsync(c => c.ScrapCategoryId);
    }
}