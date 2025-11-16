using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<PaginatedResult<ScrapPostOverralModel>> GetPosts(int pageNumber, int pageSize, Guid userId, string userRole,
        string? categoryName, PostStatus? status,
        bool sortByLocation = false, bool sortByCreateAt = false, bool sortByUpdateAt = false);

    Task<PaginatedResult<ScrapPostOverralModel>> GetPostsByHousehold(int pageNumber, int pageSize, Guid? userId, string? title,
        PostStatus? status);

    Task<ScrapPostModel> GetPost(Guid scrapPostId);
    Task<ScrapPostModel> CreateScrapPost(ScrapPostCreateModel scrapPostCreateModel);
    Task<ScrapPostModel> UpdateScrapPost(Guid userId, Guid scrapPostId, ScrapPostUpdateModel scrapPostRequestModel);
    Task<bool> ToggleScrapPost(Guid userId, Guid scrapPostId, string userRole);
}