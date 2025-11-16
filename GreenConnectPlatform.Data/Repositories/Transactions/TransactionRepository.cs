using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Transactions;

public class TransactionRepository : BaseRepository<GreenConnectDbContext, Transaction, Guid>, ITransactionRepository
{
    public TransactionRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}