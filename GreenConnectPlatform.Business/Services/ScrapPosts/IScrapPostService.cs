using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.ScrapPosts.ScrapPostDetails;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.ScrapPosts;

public interface IScrapPostService
{
    Task<PaginatedResult<ScrapPostOverralModel>> SearchPostsAsync(
        string roleName,
        int pageNumber, int pageSize,
        int? categoryId, PostStatus? status,
        bool sortByLocation, bool sortByCreateAt,
        Guid currentUserId);

    Task<PaginatedResult<ScrapPostOverralModel>> GetMyPostsAsync(
        int pageNumber, int pageSize,
        string? title, PostStatus? status,
        Guid householdId);

    Task<ScrapPostModel> GetByIdAsync(Guid id);
    Task<ScrapPostModel> CreateAsync(Guid householdId, ScrapPostCreateModel request);
    Task<ScrapPostModel> UpdateAsync(Guid householdId, Guid postId, ScrapPostUpdateModel request);
    Task ToggleStatusAsync(Guid userId, Guid postId, string userRole);
    Task AddDetailAsync(Guid householdId, Guid postId, ScrapPostDetailCreateModel detailRequest);
    Task UpdateDetailAsync(Guid householdId, Guid postId, int categoryId, ScrapPostDetailUpdateModel detailRequest);
    Task DeleteDetailAsync(Guid userId, Guid postId, int categoryId, string userRole);
}