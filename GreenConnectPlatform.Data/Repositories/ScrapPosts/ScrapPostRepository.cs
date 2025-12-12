using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Repositories.ScrapPosts;

public class ScrapPostRepository : BaseRepository<GreenConnectDbContext, ScrapPost, Guid>, IScrapPostRepository
{
    public ScrapPostRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<ScrapPost?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.Household).ThenInclude(u => u.Profile).ThenInclude(pr => pr!.Rank)
            .Include(p => p.ScrapPostDetails).ThenInclude(d => d.ScrapCategory)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.ScrapPostId == id);
    }


    public async Task<(List<ScrapPost> Items, int TotalCount)> SearchAsync(
        string roleName,
        int? categoryId,
        PostStatus? status,
        Point? userLocation,
        bool sortByLocation,
        bool sortByCreateAt,
        bool sortByUpdateAt,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking();

        if (roleName != "Admin")
            query = query.Where(p => p.Status == PostStatus.Open || p.Status == PostStatus.PartiallyBooked);

        if (status.HasValue)
            query = query.Where(s => s.Status == status.Value);

        if (categoryId != null)
        {
            query = query.Where(p =>
                p.ScrapPostDetails.Any(d => d.ScrapCategory.ScrapCategoryId == categoryId));
        }

        var totalCount = await query.CountAsync();

        if (sortByLocation && userLocation != null)
        {
            query = query
                .Where(p => p.Location != null)
                .OrderBy(p => p.Location!.Distance(userLocation));
        }
        else
        {
            if (sortByCreateAt) query = query.OrderBy(p => p.CreatedAt);
            else query = query.OrderByDescending(p => p.CreatedAt);

            if (sortByUpdateAt) query = ((IOrderedQueryable<ScrapPost>)query).ThenByDescending(p => p.UpdatedAt);
        }

        var items = await query
            .Include(p => p.Household).ThenInclude(u => u.Profile).ThenInclude(pr => pr!.Rank)
            .Include(p => p.ScrapPostDetails).ThenInclude(d => d.ScrapCategory)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<ScrapPost> Items, int TotalCount)> GetByHouseholdAsync(
        Guid householdId,
        string? title,
        PostStatus? status,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking().Where(p => p.HouseholdId == householdId);

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(p => p.Title.ToLower().Contains(title.Trim().ToLower()));

        if (status.HasValue) query = query.Where(p => p.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(p => p.Household)
            .Include(p => p.ScrapPostDetails).ThenInclude(d => d.ScrapCategory)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> IsCategoryInUseAsync(int categoryId)
    {
        return await _dbSet.AnyAsync(p => p.ScrapPostDetails.Any(d => d.ScrapCategoryId == categoryId));
    }

    public async Task<List<ScrapPost>> GetScrapPostForReport(DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate).ToListAsync();
    }

    public async Task<List<ScrapPost>> GetMyScrapPostsForReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(s => s.CreatedAt >= startDate && s.CreatedAt <= endDate && s.HouseholdId == userId)
            .ToListAsync();
    }
}