using GreenConnectPlatform.Business.Models.CreditTransactionHistories;
using GreenConnectPlatform.Business.Models.Paging;

namespace GreenConnectPlatform.Business.Services.CreditTransactionHistories;

public interface ICreditTransactionHistoryService
{
    Task<PaginatedResult<CreditTransactionHistoryModel>> GetCreditTransactionHistoriesByUserIdAsync(
        int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt, string? type = null);
}