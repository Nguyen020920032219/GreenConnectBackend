using AutoMapper;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PointHistories;
using GreenConnectPlatform.Data.Repositories.PointHistories;

namespace GreenConnectPlatform.Business.Services.PointHistories;

public class PointHistoryService : IPointHistoryService
{
    private readonly IPointHistoryRepository _pointHistoryRepository;
    private readonly IMapper _mapper;
    public PointHistoryService(IPointHistoryRepository pointHistoryRepository, IMapper mapper)
    {
        _pointHistoryRepository = pointHistoryRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedResult<PointHistoryModel>> GetPointHistoriesAsync(Guid? userId, Guid currentUserId, int pageNumber, int pageSize, bool sortDescending)
    {
        var (pointHistories, totalCount) = await _pointHistoryRepository.GetPagedPointHistoriesAsync(userId, currentUserId, sortDescending, pageNumber, pageSize);
        var pointHistoryModels = _mapper.Map<List<PointHistoryModel>>(pointHistories);
        return new PaginatedResult<PointHistoryModel>
        {
            Data = pointHistoryModels,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }
}