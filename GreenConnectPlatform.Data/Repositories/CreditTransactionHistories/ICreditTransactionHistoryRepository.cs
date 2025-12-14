using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;

public interface ICreditTransactionHistoryRepository : IBaseRepository<CreditTransactionHistory, Guid>
{
    Task<(List<CreditTransactionHistory> Items, int TotalCount)> GetCreditTransactionHistoriesByUserId(
        int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt, string? type = null);
}