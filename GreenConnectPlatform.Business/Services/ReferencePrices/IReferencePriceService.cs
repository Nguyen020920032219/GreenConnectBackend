using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ReferencePrices;

namespace GreenConnectPlatform.Business.Services.ReferencePrices;

public interface IReferencePriceService
{
    Task<PaginatedResult<ReferencePriceModel>> GetReferencePrices(int pageNumber, int pageSize, 
        string? scrapCategoryName, bool? sortByPrice, bool sortByUpdateAt);
    Task<ReferencePriceModel> GetReferencePrice(Guid referencePriceId);
    Task<ReferencePriceModel> CreateReferencePrice(int scrapCategoryId, decimal pricePerKg, Guid userId);
    Task<ReferencePriceModel> UpdateReferencePrice(Guid referencePriceId, decimal? pricePerKg, Guid userId);
    Task DeleteReferencePrice(Guid referencePriceId);
}