using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;

namespace GreenConnectPlatform.Business.Services.ScrapCategories;

public interface IScrapCategoryService
{
    Task<PaginatedResult<ScrapCategoryModel>> GetListAsync(int pageNumber, int pageSize, string? searchName);
    Task<ScrapCategoryModel> GetByIdAsync(Guid id);
    Task<ScrapCategoryModel> CreateAsync(string categoryName, string imageUrl);
    Task<ScrapCategoryModel> UpdateAsync(Guid id, string? categoryName, string? imageUrl);
    Task DeleteAsync(Guid id);
}