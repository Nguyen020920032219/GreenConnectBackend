using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.CollectionOffers;

public interface ICollectionOfferService
{
    Task<PaginatedResult<CollectionOfferOveralForCollectorModel>> GetByCollectorAsync(
        int pageNumber, int pageSize, OfferStatus? status, bool sortByCreateAtDesc, Guid collectorId);

    Task<PaginatedResult<CollectionOfferOveralForHouseModel>> GetByPostAsync(
        int pageNumber, int pageSize, OfferStatus? status, Guid postId);

    Task<CollectionOfferModel> GetByIdAsync(Guid id);
    Task<CollectionOfferModel> CreateAsync(Guid collectorId, Guid postId, CollectionOfferCreateModel request);
    Task ProcessOfferAsync(Guid householdId, Guid offerId, bool isAccepted);
    Task ToggleCancelAsync(Guid collectorId, Guid offerId);
    Task AddDetailAsync(Guid collectorId, Guid offerId, OfferDetailCreateModel detailRequest);
    Task UpdateDetailAsync(Guid collectorId, Guid offerId, Guid detailId, OfferDetailUpdateModel detailRequest);
    Task DeleteDetailAsync(Guid collectorId, Guid offerId, Guid detailId);
}