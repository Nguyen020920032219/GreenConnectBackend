using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.RewardItems;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.RewardItems;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.RewardItems;

public class RewardItemService : IRewardItemService
{
    private readonly IRewardItemRepository _rewardItemRepository;
    private readonly IMapper _mapper;

    public RewardItemService(IRewardItemRepository rewardItemRepository, IMapper mapper)
    {
        _rewardItemRepository = rewardItemRepository;
        _mapper = mapper;
    }
        
    public async Task<PaginatedResult<RewardItemModel>> GetRewardItems(int pageIndex, int pageSize, string? name, bool sortByPoint)
    {
        var (items, totalCount) = await _rewardItemRepository.GetRewardItemsAsync(pageIndex, pageSize, name, sortByPoint);
        var data = _mapper.Map<List<RewardItemModel>>(items);
        return new PaginatedResult<RewardItemModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task<RewardItemModel> GetByRewardItemIdAsync(int id)
    {
        var rewardItem = await _rewardItemRepository.GetByRewardItemIdAsync(id);
        if (rewardItem == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy phần thưởng");
        return _mapper.Map<RewardItemModel>(rewardItem);
    }

    public async Task<RewardItemModel> CreateRewardItemAsync(RewardItemCreateModel model)
    {
        var maxId = await _rewardItemRepository.GetTotalRewardItemsMaxAsync();
        var rewardItem = _mapper.Map<RewardItem>(model);
        rewardItem.RewardItemId = maxId + 1;
        await _rewardItemRepository.AddAsync(rewardItem);
        return _mapper.Map<RewardItemModel>(rewardItem);
    }

    public async Task<RewardItemModel> UpdateRewardItemAsync(int id, RewardItemUpdateModel model)
    {
        var rewardItem = await _rewardItemRepository.GetByRewardItemIdAsync(id);
        if (rewardItem == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy phần thưởng");
        if(model.PointsCost == null) model.PointsCost = rewardItem.PointsCost;
        _mapper.Map(model, rewardItem);
        await _rewardItemRepository.UpdateAsync(rewardItem);
        return _mapper.Map<RewardItemModel>(rewardItem);
    }

    public async Task DeleteRewardItemAsync(int id)
    {
        var rewardItem = await _rewardItemRepository.GetByRewardItemIdAsync(id);
        if (rewardItem == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Không tìm thấy phần thưởng");
        await _rewardItemRepository.DeleteAsync(rewardItem);
    }
}