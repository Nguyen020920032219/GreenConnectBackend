using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Notifications;

public interface INotificationRepository : IBaseRepository<Notification, Guid>
{
    Task<(List<Notification> Items, int TotalCount)> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize);
    Task<int> CountUnreadAsync(Guid userId);
}