using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Transactions.TransactionDetails;

public class TransactionDetailRepository : BaseRepository<GreenConnectDbContext, TransactionDetail, Guid>,
    ITransactionDetailRepository
{
    public TransactionDetailRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}