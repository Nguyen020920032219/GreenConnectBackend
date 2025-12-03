using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.PaymentTransactions;

public class PaymentTransactionRepository : BaseRepository<GreenConnectDbContext, PaymentTransaction, Guid>,
    IPaymentTransactionRepository
{
    public PaymentTransactionRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<PaymentTransaction?> GetByTransactionRefAsync(string transactionRef)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.TransactionRef == transactionRef);
    }
}