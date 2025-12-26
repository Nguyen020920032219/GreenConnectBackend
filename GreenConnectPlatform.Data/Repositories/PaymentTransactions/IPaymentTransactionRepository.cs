using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.PaymentTransactions;

public interface IPaymentTransactionRepository : IBaseRepository<PaymentTransaction, Guid>
{
    Task<PaymentTransaction?> GetByTransactionRefAsync(string transactionRef);

    Task<(List<PaymentTransaction> Items, int TotalCount)> GetPaymentTransactionsByUserId(
        int pageIndex, int pageSize, Guid userId, bool sortByCreatedAt, PaymentStatus? status = null);

    Task<(List<PaymentTransaction> Items, int TotalCount)> GetAllPaymentTransactions(
        int pageIndex, int pageSize, bool sortByCreatedAt, PaymentStatus? status, DateTime startDate, DateTime endDate);
}