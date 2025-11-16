using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;

namespace GreenConnectPlatform.Business.Services.Transactions.TransactionDetails;

public interface ITransactionDetailService
{
    Task<List<TransactionDetailModel>> CreateTransactionDetails(Guid transactionId,Guid scrapCollectorId, List<TransactionDetailCreateModel> transactionDetailCreateModels);
    Task<TransactionDetailModel> UpdateTransactionDetail(Guid scrapCollectorId, Guid transactionId, int scrapCategoryId, TransactionDetailUpdateModel transactionDetailUpdateModel);
}