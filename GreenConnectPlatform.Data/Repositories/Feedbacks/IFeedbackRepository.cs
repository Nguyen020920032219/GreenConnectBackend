using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Feedbacks;

public interface IFeedbackRepository : IBaseRepository<Feedback, Guid>
{
    Task<(List<Feedback> Items, int TotalCount)> GetFeedbackByTransactionId(int pageIndex, int pageSize,
        Guid transactionId, bool sortByCreateAt);

    Task<(List<Feedback> Items, int TotalCount)> GetMyFeedback(int pageIndex, int pageSize, Guid userId,
        string roleName, bool sortByCreateAt);

    Task<Feedback?> GetFeedbackById(Guid id);

    Task<List<Feedback>> GetAllFeedbacks(Guid userId, DateTime startDate, DateTime endDate);
}