using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.ReferencePrices;

public class ReferencePriceRepository : BaseRepository<GreenConnectDbContext, ReferencePrice, Guid>, IReferencePriceRepository
{
    public ReferencePriceRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<ReferencePrice?> GetReferencePriceById(Guid referencePriceId)
    {
        return await _dbSet
            .Include(r => r.ScrapCategory)
            .Include(r => r.UpdatedByAdmin)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.ReferencePriceId == referencePriceId);
    }

    public async Task<ReferencePrice?> GetReferencePriceByCategoryId(int categoryId)
    {
        return await _dbSet
            .Include(r => r.ScrapCategory)
            .Include(r => r.UpdatedByAdmin)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.ScrapCategoryId == categoryId);
    }

    public async Task<(List<ReferencePrice> Items, int TotalCount)> GetReferencePrices(int pageIndex, int pageSize, string? scrapCategoryName, bool? sortByPrice, bool sortByUpdateAt)
    {
        var query = _dbSet.AsNoTracking();
        if (!string.IsNullOrEmpty(scrapCategoryName))
        {
            query = query.Where(r => r.ScrapCategory.CategoryName.ToLower().Contains(scrapCategoryName.ToLower()));
        }
        if (sortByPrice.HasValue)
        {
            query = query.OrderByDescending(r => r.PricePerKg);
        }
        else
        {
            query = query.OrderBy(r => r.PricePerKg);
        }
        if (sortByUpdateAt)
        {
            query = query.OrderByDescending(r => r.LastUpdated);
        }
        else
        {
            query = query.OrderBy(r => r.LastUpdated);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}