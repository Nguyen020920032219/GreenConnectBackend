using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Feedbacks;

public class FeedbackRepository : BaseRepository<GreenConnectDbContext, Feedback, Guid>, IFeedbackRepository
{
    public FeedbackRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<(List<Feedback> Items, int TotalCount)> GetFeedbackByTransactionId(int pageIndex, int pageSize, Guid transactionId, bool sortByCreateAt)
    {
        var query = _dbSet
            .Where(f => f.TransactionId == transactionId)
            .AsNoTracking();
        if (sortByCreateAt)
            query = query.OrderByDescending(f => f.CreatedAt);
        else
            query = query.OrderBy(f => f.CreatedAt);
        int totalCount = await query.CountAsync();
        var items = await query
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Household)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.ScrapCollector)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(o => o.ScrapPost)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.TransactionDetails)
            .Include(f => f.Reviewer)
            .Include(f => f.Reviewee)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<(List<Feedback> Items, int TotalCount)> GetMyFeedback(int pageIndex, int pageSize, Guid userId, string roleName, bool sortByCreateAt)
    {
        var query = _dbSet.AsNoTracking();
        if (roleName == "Household")
        {
            query = query.Where(f => f.ReviewerId == userId);
        }

        if (roleName == "IndividualCollector" || roleName == "BusinessCollector")
        {
            query = query.Where(f => f.RevieweeId == userId);
        }

        if (sortByCreateAt)
        {
            query = query.OrderByDescending(f => f.CreatedAt);
        }
        else
        {
            query = query.OrderBy(f => f.CreatedAt);
        }
        int totalCount = await query.CountAsync();
        var items = await query
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Household)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.ScrapCollector)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(o => o.ScrapPost)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.TransactionDetails)
            .Include(f => f.Reviewer)
            .Include(f => f.Reviewee)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<Feedback?> GetFeedbackById(Guid id)
    {
        return await _dbSet
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Household)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.ScrapCollector)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(o => o.ScrapPost)
            .Include(f => f.Transaction)
            .ThenInclude(t => t.TransactionDetails)
            .Include(f => f.Reviewer)
            .Include(f => f.Reviewee)
            .FirstOrDefaultAsync(f => f.FeedbackId == id);
    }
}