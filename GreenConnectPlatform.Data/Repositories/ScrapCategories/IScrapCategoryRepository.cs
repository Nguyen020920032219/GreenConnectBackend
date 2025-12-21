using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScrapCategories;

public interface IScrapCategoryRepository : IBaseRepository<ScrapCategory, int>
{
    Task<(IList<ScrapCategory> Items, int TotalCount)> SearchAndPaginateAsync(string? keyword, int pageIndex,
        int pageSize);

    Task<ScrapCategory?> GetByIdAsync(Guid id);
}