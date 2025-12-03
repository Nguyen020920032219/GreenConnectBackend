using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;

public class CreditTransactionHistoryRepository : BaseRepository<GreenConnectDbContext, CreditTransactionHistory, Guid>,
    ICreditTransactionHistoryRepository
{
    public CreditTransactionHistoryRepository(GreenConnectDbContext context) : base(context)
    {
    }
}