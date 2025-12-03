using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CreditTransactionHistories;

public interface ICreditTransactionHistoryRepository : IBaseRepository<CreditTransactionHistory, Guid>
{
}