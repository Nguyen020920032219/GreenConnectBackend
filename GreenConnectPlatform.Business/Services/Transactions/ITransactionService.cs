using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;

namespace GreenConnectPlatform.Business.Services.Transactions;

public interface ITransactionService
{
    Task CheckIn(LocationModel location,Guid transactionId, Guid userId);
    Task<TransactionModel> GetTransaction(Guid transactionId);
    Task<PaginatedResult<TransactionOveralModel>> GetTransactionsByUserId(Guid userId,string roleName, 
        int pageNumber, int pageSize, bool sortByCreateAt = false, bool sortByUpdateAt = false);
    
    Task CancelOrAcceptTransaction(Guid transactionId, Guid userId, bool isAccept);
    Task CancelOrReopenTransaction(Guid transactionId, Guid userId);
}