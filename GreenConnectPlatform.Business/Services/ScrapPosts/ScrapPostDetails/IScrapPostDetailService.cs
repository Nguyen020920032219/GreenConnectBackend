using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;

namespace GreenConnectPlatform.Business.Services.ScrapPosts.ScrapPostDetails;

public interface IScrapPostDetailService
{
    Task<ScrapPostDetailModel> GetScrapPostDetailById(Guid scrapPostId, int scrapCategoryId);

    Task<ScrapPostDetailModel> AddScrapPostDetail(Guid userId, Guid scrapPostId,
        ScrapPostDetailCreateModel crapPostDetailCreateModel);

    Task<ScrapPostDetailModel> UpdateScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId,
        ScrapPostDetailUpdateModel scrapPostDetailCreateModel);

    Task DeleteScrapPostDetail(Guid userId, Guid scrapPostId, string userRole, int scrapCategoryId);
}