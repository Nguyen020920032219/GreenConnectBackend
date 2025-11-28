using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.CollectionOffers;

public class CollectionOfferRepository : BaseRepository<GreenConnectDbContext, CollectionOffer, Guid>,
    ICollectionOfferRepository
{
    public CollectionOfferRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<CollectionOffer?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(o => o.ScrapCollector).ThenInclude(u => u.Profile).ThenInclude(p => p!.Rank)
            .Include(o => o.OfferDetails).ThenInclude(od => od.ScrapCategory)
            .Include(o => o.ScheduleProposals)
            .Include(o => o.ScrapPost).ThenInclude(p => p.ScrapPostDetails)
            .Include(o => o.ScrapPost).ThenInclude(p => p.Household)
            .Include(o => o.Transactions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.CollectionOfferId == id);
    }

    public async Task<(List<CollectionOffer> Items, int TotalCount)> GetByCollectorAsync(
        Guid collectorId,
        OfferStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking().Where(o => o.ScrapCollectorId == collectorId);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        var totalCount = await query.CountAsync();

        if (sortByCreateAtDesc) query = query.OrderByDescending(o => o.CreatedAt);
        else query = query.OrderBy(o => o.CreatedAt);

        var items = await query
            .Include(o => o.ScrapPost).ThenInclude(p => p.Household)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<CollectionOffer> Items, int TotalCount)> GetByPostIdAsync(
        Guid postId,
        OfferStatus? status,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking().Where(o => o.ScrapPostId == postId);

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .Include(o => o.ScrapCollector).ThenInclude(u => u.Profile).ThenInclude(p => p!.Rank)
            .Include(o => o.ScrapPost)
            .Include(o => o.OfferDetails)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<CollectionOffer>> GetOffersForReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(o => o.ScrapCollectorId == userId && o.CreatedAt >= startDate && o.CreatedAt <= endDate)
            .ToListAsync();
    }
}