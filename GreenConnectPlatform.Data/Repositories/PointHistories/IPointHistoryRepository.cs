using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.PointHistories;

public interface IPointHistoryRepository : IBaseRepository<PointHistory, Guid>
{
    Task<(List<PointHistory> Items, int TotalCount)> GetPagedPointHistoriesAsync(
        Guid? userId,
        Guid currentUserId,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize);
}