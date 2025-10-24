using GreenConnectPlatform.Bussiness.Models.ScrapPosts.ScrapPostDetails;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts.ScrapPostDetails;

public interface IScrapPostDetailService
{
    Task<ScrapPostDetailModel> GetScrapPostDetailById(Guid scrapPostId, int scrapCategoryId);

    Task<ScrapPostDetailModel> CreateScrapPostDetail(Guid userId, Guid scrapPostId,
        ScrapPostDetailCreateModel crapPostDetailCreateModel);

    Task<ScrapPostDetailModel> UpdateScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId,
        ScrapPostDetailUpdateModel scrapPostDetailCreateModel);

    Task<bool> DeleteScrapPostDetail(Guid userId, Guid scrapPostId, int scrapCategoryId);
}