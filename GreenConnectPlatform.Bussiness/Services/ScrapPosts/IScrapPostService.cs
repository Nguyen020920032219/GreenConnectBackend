using GreenConnectPlatform.Bussiness.Models.ScrapPosts;
using GreenConnectPlatform.Data.Entities;

namespace GreenConnectPlatform.Bussiness.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<List<ScrapPostModel>> GetPosts(int pageNumber, int pageSize);
    Task<ScrapPostModel> GetPost(Guid scrapPostId);
    Task<ScrapPostModel> CreateScrapPost(ScrapPostRequest scrapPostRequest);
}