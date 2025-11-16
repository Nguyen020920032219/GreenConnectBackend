using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Transactions;

public interface ITransactionRepository : IBaseRepository<Transaction, Guid>
{
    
}