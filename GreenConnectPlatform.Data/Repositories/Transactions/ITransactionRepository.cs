using GreenConnectPlatform.Data.Entities;
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
    
    Task<List<Transaction>> GetTransactionsForReport(DateTime startDate, DateTime endDate);
}