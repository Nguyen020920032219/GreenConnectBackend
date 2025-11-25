using GreenConnectPlatform.Business.Models.Feedbacks;
using GreenConnectPlatform.Business.Models.Paging;

namespace GreenConnectPlatform.Business.Services.Feedbacks;

public interface IFeedbackService
{
    Task<PaginatedResult<FeedbackModel>> GetFeedbacksAsync(int pageNumber, int pageSize, Guid transactionId, bool sortByCreatAt);
    Task<PaginatedResult<FeedbackModel>> GetFeedbacksByUserIdAsync(int pageNumber, int pageSize, Guid userId, string roleName, bool sortByCreatAt);
    Task<FeedbackModel> GetFeedbackByIdAsync(Guid id);
    Task<FeedbackModel> CreateFeedbackAsync(Guid userId, FeedbackCreateModel feedback);
    Task<FeedbackModel> UpdateFeedbackAsync(Guid id, Guid userId, int? rate, string? comment);
    Task DeleteFeedbackAsync(Guid id, Guid userId);
}