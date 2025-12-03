using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Transactions.TransactionDetails;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.Transactions;

public interface ITransactionService
{
    Task CheckInAsync(Guid transactionId, Guid collectorId, LocationModel currentLocation);
    Task<TransactionModel> GetByIdAsync(Guid id);

    Task<PaginatedResult<TransactionOveralModel>> GetByOfferIdAsync(
        Guid offerId,
        TransactionStatus? status,
        bool sortByCreateAtDesc,
        bool sortByUpdateAtDesc,
        int pageIndex,
        int pageSize);

    Task<PaginatedResult<TransactionOveralModel>> GetListAsync(
        Guid userId, string role, int pageNumber, int pageSize, bool sortByCreateAt, bool sortByUpdateAt);

    Task<List<TransactionDetailModel>> SubmitDetailsAsync(
        Guid transactionId, Guid collectorId, List<TransactionDetailCreateModel> details);

    Task ProcessTransactionAsync(Guid transactionId, Guid householdId, bool isAccepted);
    Task ToggleCancelAsync(Guid transactionId, Guid collectorId);

    Task<string> GetTransactionQrCodeAsync(Guid transactionId, Guid userId);
}