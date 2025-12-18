// using AutoMapper;
// using GreenConnectPlatform.Business.Models.Exceptions;
// using GreenConnectPlatform.Business.Models.Paging;
// using GreenConnectPlatform.Business.Models.ReferencePrices;
// using GreenConnectPlatform.Data.Entities;
// using GreenConnectPlatform.Data.Repositories.ReferencePrices;
// using GreenConnectPlatform.Data.Repositories.ScrapCategories;
// using Microsoft.AspNetCore.Http;
//
// namespace GreenConnectPlatform.Business.Services.ReferencePrices;
//
// public class ReferencePriceService : IReferencePriceService
// {
//     private readonly IMapper _mapper;
//     private readonly IReferencePriceRepository _referencePriceRepository;
//     private readonly IScrapCategoryRepository _scrapCategoryRepository;
//
//     public ReferencePriceService(IReferencePriceRepository referencePriceRepository,
//         IScrapCategoryRepository categoryRepository, IMapper mapper)
//     {
//         _referencePriceRepository = referencePriceRepository;
//         _scrapCategoryRepository = categoryRepository;
//         _mapper = mapper;
//     }
//
//     public async Task<PaginatedResult<ReferencePriceModel>> GetReferencePrices(int pageNumber, int pageSize,
//         string? scrapCategoryName, bool? sortByPrice,
//         bool sortByUpdateAt)
//     {
//         var (items, totalCount) = await _referencePriceRepository.GetReferencePrices(pageNumber, pageSize,
//             scrapCategoryName, sortByPrice, sortByUpdateAt);
//
//         var referencePriceModels = _mapper.Map<List<ReferencePriceModel>>(items);
//
//         return new PaginatedResult<ReferencePriceModel>
//         {
//             Data = referencePriceModels,
//             Pagination = new PaginationModel(totalCount, pageNumber, pageSize)
//         };
//     }
//
//     public async Task<ReferencePriceModel> GetReferencePrice(Guid referencePriceId)
//     {
//         var referencePrice = await _referencePriceRepository.GetReferencePriceById(referencePriceId);
//         if (referencePrice == null)
//             throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Giá tham khảo không tồn tại");
//         return _mapper.Map<ReferencePriceModel>(referencePrice);
//     }
//
