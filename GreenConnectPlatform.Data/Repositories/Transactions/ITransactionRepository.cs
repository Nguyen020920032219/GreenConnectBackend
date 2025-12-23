using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction, Guid>
{
    Task<Transaction?> GetByIdWithDetailsAsync(Guid id);

    Task<(List<Transaction> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId,
        string role,
        bool sortByCreateAtDesc,
        bool sortByUpdateAtDesc,
        int pageIndex,
        int pageSize);

    Task<(List<Transaction> Items, int TotalCount)> GetByOfferIdAsync(
        Guid offerId,
        TransactionStatus? status,
        bool sortByCreateAtDesc,
        bool sortByUpdateAtDesc,
        int pageIndex,
        int pageSize);

    Task<List<Transaction>> GetTransactionsForReport(DateTime startDate, DateTime endDate);

    Task<List<Transaction>> GetEarningForCollectorReport(Guid userId, DateTime startDate, DateTime endDate);

    Task<List<Transaction>> GetTransactionsForCollectorReport(Guid userId, DateTime startDate, DateTime endDate);

    Task<List<Transaction>> GetTransactionByIdsAsync(Guid collectorId, Guid postId, Guid timeSlotId);

    Task UpdateRangeAsync(List<Transaction> transactions);
}