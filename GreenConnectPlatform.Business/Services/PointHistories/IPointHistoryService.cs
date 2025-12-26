using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PointHistories;

namespace GreenConnectPlatform.Business.Services.PointHistories;

public interface IPointHistoryService
{
    Task<PaginatedResult<PointHistoryModel>> GetPointHistoriesAsync(Guid? userId, Guid currentUserId, int pageNumber,
        int pageSize, bool sortDescending);
}