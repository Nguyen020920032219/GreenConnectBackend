using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Complaints;

public class ComplaintRepository : BaseRepository<GreenConnectDbContext, Complaint, Guid>, IComplaintRepository
{
    public ComplaintRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Complaint?> GetComplaintByIdAsync(Guid complaintId)
    {
        return await _dbSet
            .Include(c => c.Complainant)
            .ThenInclude(c => c.Profile)
            .Include(c => c.Accused)
            .ThenInclude(c => c.Profile)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Household)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.ScrapCollector)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.TransactionDetails)
            .ThenInclude(t => t.ScrapCategory)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(t => t.OfferDetails)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(t => t.ScrapPost)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            // .ThenInclude(t => t.ScheduleProposals)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.ComplaintId == complaintId);
    }

    public async Task<(List<Complaint> Items, int TotalCount)> GetComplaintsAsync(int pageIndex, int pageSize,
        bool sortByCreatedAt, ComplaintStatus? sortByStatus, Guid? userId,
        string? roleName)
    {
        var query = _dbSet.AsNoTracking();
        if (userId.HasValue && roleName != "Admin") query = query.Where(c => c.ComplainantId == userId);
        if (sortByCreatedAt)
            query = query.OrderByDescending(c => c.CreatedAt);
        else
            query = query.OrderBy(c => c.CreatedAt);
        if (sortByStatus.HasValue) query = query.Where(c => c.Status == sortByStatus.Value);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(c => c.Complainant)
            .Include(c => c.Accused)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Household)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.ScrapCollector)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.TransactionDetails)
            .ThenInclude(t => t.ScrapCategory)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(t => t.OfferDetails)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            .ThenInclude(t => t.ScrapPost)
            .Include(c => c.Transaction)
            .ThenInclude(t => t.Offer)
            // .ThenInclude(t => t.ScheduleProposals)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<List<Complaint>> GetComplaintsForReport(DateTime startDate, DateTime endDate)
    {
        return await _dbSet.Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate).ToListAsync();
    }

    public async Task<List<Complaint>> GetComplaintsForCollectorReport(Guid userId, DateTime startDate,
        DateTime endDate)
    {
        return await _dbSet
            .Where(c => c.ComplainantId == userId && c.CreatedAt >= startDate && c.CreatedAt <= endDate).ToListAsync();
    }

    public async Task<List<Complaint>> GetAccusedForCollectorReport(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(c => c.AccusedId == userId && c.CreatedAt >= startDate && c.CreatedAt <= endDate).ToListAsync();
    }
}