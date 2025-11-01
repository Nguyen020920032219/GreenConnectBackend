using GreenConnectPlatform.Bussiness.Models.ScrapPosts;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<List<ScrapPostOverral>> GetPosts(int pageNumber, int pageSize);
    Task<ScrapPostModel> GetPost(Guid scrapPostId);
    Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest);
}