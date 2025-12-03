using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.PaymentTransactions;

public interface IPaymentTransactionRepository : IBaseRepository<PaymentTransaction, Guid>
{
    Task<PaymentTransaction?> GetByTransactionRefAsync(string transactionRef);
}