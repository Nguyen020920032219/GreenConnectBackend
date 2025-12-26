using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Notifications;

public class NotificationRepository : BaseRepository<GreenConnectDbContext, Notification, Guid>, INotificationRepository
{
    public NotificationRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<(List<Notification> Items, int TotalCount)> GetByUserIdAsync(Guid userId, int pageIndex,
        int pageSize)
    {
        var query = _dbSet.AsNoTracking()
            .Where(n => n.RecipientId == userId)
            .OrderByDescending(n => n.CreatedAt);

        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> CountUnreadAsync(Guid userId)
    {
        return await _dbSet.CountAsync(n => n.RecipientId == userId && !n.IsRead);
    }
}