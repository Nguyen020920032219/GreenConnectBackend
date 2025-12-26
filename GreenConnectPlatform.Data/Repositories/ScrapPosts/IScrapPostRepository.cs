using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;
using NetTopologySuite.Geometries;

namespace GreenConnectPlatform.Data.Repositories.ScrapPosts;

public interface IScrapPostRepository : IBaseRepository<ScrapPost, Guid>
{
    Task<ScrapPost?> GetByIdWithDetailsAsync(Guid id);

    Task<(List<ScrapPost> Items, int TotalCount)> SearchAsync(
        string roleName,
        Guid? categoryId,
        PostStatus? status,
        Point? userLocation,
        bool sortByLocation,
        bool sortByCreateAt,
        bool sortByUpdateAt,
        int pageIndex,
        int pageSize);

    Task<(List<ScrapPost> Items, int TotalCount)> GetByHouseholdAsync(
        Guid householdId,
        string? title,
        PostStatus? status,
        int pageIndex,
        int pageSize);

    Task<bool> IsCategoryInUseAsync(Guid categoryId);

    Task<List<ScrapPost>> GetScrapPostForReport(DateTime startDate, DateTime endDate);
    Task<List<ScrapPost>> GetMyScrapPostsForReport(Guid userId, DateTime startDate, DateTime endDate);
}