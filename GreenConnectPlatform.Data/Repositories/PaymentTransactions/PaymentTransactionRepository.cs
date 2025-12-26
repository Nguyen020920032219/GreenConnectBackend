using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.PaymentTransactions;

public class PaymentTransactionRepository : BaseRepository<GreenConnectDbContext, PaymentTransaction, Guid>,
    IPaymentTransactionRepository
{
    public PaymentTransactionRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<PaymentTransaction?> GetByTransactionRefAsync(string transactionRef)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TransactionRef == transactionRef);
    }

    public async Task<(List<PaymentTransaction> Items, int TotalCount)> GetPaymentTransactionsByUserId(int pageIndex,
        int pageSize, Guid userId, bool sortByCreatedAt,
        PaymentStatus? status = null)
    {
        var query = _dbSet
            .Where(pt => pt.UserId == userId)
            .AsNoTracking();
        if (sortByCreatedAt)
            query = query.OrderByDescending(t => t.CreatedAt);
        else
            query = query.OrderBy(t => t.CreatedAt);
        if (status != null)
            query = query.Where(t => t.Status == status);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(t => t.User)
            .ThenInclude(u => u.Profile)
            .Include(t => t.Package)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<(List<PaymentTransaction> Items, int TotalCount)> GetAllPaymentTransactions(int pageIndex,
        int pageSize, bool sortByCreatedAt, PaymentStatus? status, DateTime startDate,
        DateTime endDate)
    {
        var query = _dbSet
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .AsNoTracking();
        if (sortByCreatedAt)
            query = query.OrderByDescending(t => t.CreatedAt);
        else
            query = query.OrderBy(t => t.CreatedAt);
        if (status != null)
            query = query.Where(t => t.Status == status);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(t => t.User)
            .ThenInclude(u => u.Profile)
            .Include(t => t.Package)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }
}