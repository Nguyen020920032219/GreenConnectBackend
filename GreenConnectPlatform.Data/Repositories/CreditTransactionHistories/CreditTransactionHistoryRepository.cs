using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;

public class CreditTransactionHistoryRepository : BaseRepository<GreenConnectDbContext, CreditTransactionHistory, Guid>,
    ICreditTransactionHistoryRepository
{
    public CreditTransactionHistoryRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<(List<CreditTransactionHistory> Items, int TotalCount)> GetCreditTransactionHistoriesByUserId(
        int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt,
        string? type = null)
    {
        var query = _dbSet
            .Where(cth => cth.UserId == userId)
            .AsNoTracking();
        if (sortByCreatedAt)
            query = query.OrderByDescending(cth => cth.CreatedAt);
        else
            query = query.OrderBy(cth => cth.CreatedAt);
        if (!string.IsNullOrEmpty(type))
            query = query.Where(cth => cth.Type == type);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(cth => cth.User)
            .ThenInclude(u => u.Profile)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}