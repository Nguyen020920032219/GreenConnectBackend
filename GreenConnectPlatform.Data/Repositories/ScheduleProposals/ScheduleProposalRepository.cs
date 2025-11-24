using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.ScheduleProposals;

public class ScheduleProposalRepository : BaseRepository<GreenConnectDbContext, ScheduleProposal, Guid>,
    IScheduleProposalRepository
{
    public ScheduleProposalRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<ScheduleProposal?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.Offer).ThenInclude(o => o.ScrapPost).ThenInclude(p => p.Household)
            .Include(s => s.Offer).ThenInclude(o => o.ScrapCollector)
            .FirstOrDefaultAsync(s => s.ScheduleProposalId == id);
    }

    public async Task<(List<ScheduleProposal> Items, int TotalCount)> GetByOfferAsync(
        Guid offerId,
        ProposalStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking().Where(s => s.CollectionOfferId == offerId);

        if (status.HasValue) query = query.Where(s => s.Status == status.Value);

        var totalCount = await query.CountAsync();

        if (sortByCreateAtDesc) query = query.OrderByDescending(s => s.CreatedAt);
        else query = query.OrderBy(s => s.CreatedAt);

        var items = await query
            .Include(s => s.Offer)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<ScheduleProposal> Items, int TotalCount)> GetByCollectorAsync(
        Guid collectorId,
        ProposalStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking().Where(s => s.ProposerId == collectorId);

        if (status.HasValue) query = query.Where(s => s.Status == status.Value);

        var totalCount = await query.CountAsync();

        if (sortByCreateAtDesc) query = query.OrderByDescending(s => s.CreatedAt);
        else query = query.OrderBy(s => s.CreatedAt);

        var items = await query
            .Include(s => s.Offer).ThenInclude(o => o.ScrapPost)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<ScheduleProposal>> GetByOffer(Guid offerId)
    {
        return await _dbSet.Where(p => p.CollectionOfferId == offerId).ToListAsync();
    }
}