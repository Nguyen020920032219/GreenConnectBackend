using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ReferencePrices;

public interface IReferencePriceRepository : IBaseRepository<ReferencePrice, Guid>
{
    Task<ReferencePrice?> GetReferencePriceById(Guid referencePriceId);
    Task<ReferencePrice?> GetReferencePriceByCategoryId(Guid categoryId);

    Task<(List<ReferencePrice> Items, int TotalCount)> GetReferencePrices(
        int pageIndex,
        int pageSize,
        string? scrapCategoryName,
        bool? sortByPrice,
        bool sortByUpdateAt);
}