using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ReferencePrices;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.ReferencePrices;
using GreenConnectPlatform.Data.Repositories.ScrapCategories;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.ReferencePrices;

public class ReferencePriceService : IReferencePriceService
{
    private readonly IReferencePriceRepository _referencePriceRepository;
    private readonly IScrapCategoryRepository _scrapCategoryRepository;
    private readonly IMapper _mapper;
    
    public ReferencePriceService(IReferencePriceRepository referencePriceRepository,IScrapCategoryRepository categoryRepository, IMapper mapper)
    {
        _referencePriceRepository = referencePriceRepository;
        _scrapCategoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<PaginatedResult<ReferencePriceModel>> GetReferencePrices(int pageNumber, int pageSize, string? scrapCategoryName, bool? sortByPrice,
        bool sortByUpdateAt)
    {
        var (items, totalCount) = await _referencePriceRepository.GetReferencePrices(pageNumber, pageSize, scrapCategoryName, sortByPrice, sortByUpdateAt);
        
        var referencePriceModels = _mapper.Map<List<ReferencePriceModel>>(items);

        return new PaginatedResult<ReferencePriceModel>
        {
            Data = referencePriceModels,
            Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
        };
    }

    public async Task<ReferencePriceModel> GetReferencePrice(Guid referencePriceId)
    {
        var referencePrice = await _referencePriceRepository.GetReferencePriceById(referencePriceId);
        if (referencePrice == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giá tham khảo không tồn tại");
        return _mapper.Map<ReferencePriceModel>(referencePrice);
    }

    public async Task<ReferencePriceModel> CreateReferencePrice(int scrapCategoryId, decimal pricePerKg, Guid userId)
    {
        var scrapCategory = await _scrapCategoryRepository.GetByIdAsync(scrapCategoryId);
        if(scrapCategory == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Loại phế liệu không tồn tại");
        var existingReferencePrice = await _referencePriceRepository.GetReferencePriceByCategoryId(scrapCategoryId);
        if(existingReferencePrice != null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Giá tham khảo cho loại phế liệu này đã tồn tại");
        if(pricePerKg <= 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Giá cho 1 kg phải lớn hơn 0");
        var referencePrice = new ReferencePrice
        {
            ReferencePriceId = Guid.NewGuid(),
            ScrapCategoryId = scrapCategoryId,
            PricePerKg = pricePerKg,
            LastUpdated = DateTime.UtcNow,
            UpdatedByAdminId = userId
        };
        await _referencePriceRepository.AddAsync(referencePrice);
        return await GetReferencePrice(referencePrice.ReferencePriceId);
    }

    public async Task<ReferencePriceModel> UpdateReferencePrice(Guid referencePriceId, decimal? pricePerKg, Guid userId)
    {
        var referencePrice = await _referencePriceRepository.GetReferencePriceById(referencePriceId);
        if (referencePrice == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giá tham khảo không tồn tại");
        if (pricePerKg.HasValue && pricePerKg.Value <= 0)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Giá tham khảo cho 1 kg phải lớn hơn 0");
        if (pricePerKg.HasValue)
            referencePrice.PricePerKg = pricePerKg.Value;
        if(referencePrice.UpdatedByAdminId != userId)
            referencePrice.UpdatedByAdminId = userId;
        referencePrice.LastUpdated = DateTime.UtcNow;
        await _referencePriceRepository.UpdateAsync(referencePrice);
        return _mapper.Map<ReferencePriceModel>(referencePrice);
    }

    public async Task DeleteReferencePrice(Guid referencePriceId)
    {
        var referencePrice = await _referencePriceRepository.GetReferencePriceById(referencePriceId);
        if (referencePrice == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giá tham khảo không tồn tại");
        await _referencePriceRepository.DeleteAsync(referencePrice);
    }
}