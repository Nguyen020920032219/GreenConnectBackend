using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentTransactions;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.PaymentTransactions;

public interface IPaymentTransactionService
{
    Task<PaginatedResult<PaymentTransactionModel>> GetPaymentTransactionsByUserAsync(int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt, PaymentStatus? status = null);
    Task<PaginatedResult<PaymentTransactionModel>> GetPaymentTransactionsAsync(int pageIndex, int pageSize, bool sortByCreatedAt, PaymentStatus? status, DateTime startDate, DateTime endDate);
}