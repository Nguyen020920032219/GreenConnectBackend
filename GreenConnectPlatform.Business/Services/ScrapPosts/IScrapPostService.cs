using GreenConnectPlatform.Business.Models.ScrapPosts;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<List<ScrapPostOverral>> GetPosts(int pageNumber, int pageSize);
    Task<ScrapPostModel> GetPost(Guid scrapPostId);
    Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest);
}