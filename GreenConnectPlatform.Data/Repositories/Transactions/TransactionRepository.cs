using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Transactions;

public class TransactionRepository : BaseRepository<GreenConnectDbContext, Transaction, Guid>, ITransactionRepository
{
    public TransactionRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<Transaction?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.TransactionDetails).ThenInclude(d => d.ScrapCategory)
            .Include(t => t.Household).ThenInclude(u => u.Profile)
            .Include(t => t.ScrapCollector).ThenInclude(u => u.Profile)
            .Include(t => t.Offer).ThenInclude(o => o.OfferDetails)
            .Include(t => t.Offer).ThenInclude(o => o.ScrapPost).ThenInclude(s => s.ScrapPostDetails)
            // .Include(t => t.Offer).ThenInclude(o => o.ScheduleProposals)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.TransactionId == id);
    }

    public async Task<(List<Transaction> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId,
        string role,
        bool sortByCreateAtDesc,
        bool sortByUpdateAtDesc,
        int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking();

        if (role == "Household")
            query = query.Where(t => t.HouseholdId == userId);
        else
            query = query.Where(t => t.ScrapCollectorId == userId);

        var totalCount = await query.CountAsync();

        if (sortByUpdateAtDesc)
            query = query.OrderByDescending(t => t.UpdatedAt);
        else if (sortByCreateAtDesc)
            query = query.OrderByDescending(t => t.CreatedAt);
        else
            query = query.OrderBy(t => t.CreatedAt);

        var items = await query
            .Include(t => t.TransactionDetails).ThenInclude(d => d.ScrapCategory)
            .Include(t => t.Household).ThenInclude(u => u.Profile)
            .Include(t => t.ScrapCollector).ThenInclude(u => u.Profile)
            .Include(t => t.Offer).ThenInclude(o => o.OfferDetails)
            .Include(t => t.Offer).ThenInclude(o => o.ScrapPost).ThenInclude(s => s.ScrapPostDetails)
            // .Include(t => t.Offer).ThenInclude(o => o.ScheduleProposals)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Transaction> Items, int TotalCount)> GetByOfferIdAsync(Guid offerId,
        TransactionStatus? status, bool sortByCreateAtDesc, bool sortByUpdateAtDesc, int pageIndex, int pageSize)
    {
        var query = _dbSet.AsNoTracking()
            .Where(t => t.OfferId == offerId);

        var totalCount = await query.CountAsync();
        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);
        if (sortByUpdateAtDesc)
            query = query.OrderByDescending(t => t.UpdatedAt);
        else if (sortByCreateAtDesc)
            query = query.OrderByDescending(t => t.CreatedAt);
        else
            query = query.OrderBy(t => t.CreatedAt);

        var items = await query
            .Include(t => t.TransactionDetails).ThenInclude(d => d.ScrapCategory)
            .Include(t => t.Household).ThenInclude(u => u.Profile)
            .Include(t => t.ScrapCollector).ThenInclude(u => u.Profile)
            .Include(t => t.Offer).ThenInclude(o => o.OfferDetails)
            .Include(t => t.Offer).ThenInclude(o => o.ScrapPost).ThenInclude(s => s.ScrapPostDetails)
            // .Include(t => t.Offer).ThenInclude(o => o.ScheduleProposals)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Transaction>> GetTransactionsForReport(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetEarningForCollectorReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.TransactionDetails)
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate && t.Status == TransactionStatus.Completed &&
                        t.ScrapCollectorId == userId)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetTransactionsForCollectorReport(Guid userId, DateTime startDate,
        DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.TransactionDetails)
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate && t.ScrapCollectorId == userId)
            .ToListAsync();
    }
}