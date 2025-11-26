using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;

namespace GreenConnectPlatform.Business.Services.ScrapCategories;

public interface IScrapCategoryService
{
    Task<PaginatedResult<ScrapCategoryModel>> GetListAsync(int pageNumber, int pageSize, string? searchName);
    Task<ScrapCategoryModel> GetByIdAsync(int id);
    Task<ScrapCategoryModel> CreateAsync(string categoryName, string description);
    Task<ScrapCategoryModel> UpdateAsync(int id, string? categoryName, string? description);
    Task DeleteAsync(int id);
}