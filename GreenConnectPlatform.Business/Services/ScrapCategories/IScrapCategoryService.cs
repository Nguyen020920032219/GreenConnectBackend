using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapCategories;

namespace GreenConnectPlatform.Business.Services.ScrapCategories;

public interface IScrapCategoryService
{
    Task<PaginatedResult<ScrapCategoryModel>> GetScrapCategories(int pageNumber, int pageSize, string? categoryName);
    Task<ScrapCategoryModel> GetScrapCategory(int scrapCategoryId);
}