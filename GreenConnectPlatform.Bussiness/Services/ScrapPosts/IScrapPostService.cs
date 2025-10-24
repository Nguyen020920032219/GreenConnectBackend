using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<List<ScrapPostOverralModel>> GetPosts(int pageNumber, int pageSize, Guid? userId, string? categoryName,
        bool sortByLocation = false);

    Task<List<ScrapPostOverralModel>> GetPostsByHousehold(int pageNumber, int pageSize, Guid? userId, string? title,
        PostStatus? status);

    Task<ScrapPostModel> GetPost(Guid scrapPostId);
    Task<ScrapPostModel> CreateScrapPost(ScrapPostCreateModel scrapPostCreateModel);
    Task<ScrapPostModel> UpdateScrapPost(Guid userId, Guid scrapPostId, ScrapPostUpdateModel scrapPostRequestModel);
    Task<bool> ToggleScrapPost(Guid userId, Guid scrapPostId);
}