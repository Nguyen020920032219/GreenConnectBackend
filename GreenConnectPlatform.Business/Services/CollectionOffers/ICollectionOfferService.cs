using GreenConnectPlatform.Business.Models.CollectionOffers;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.CollectionOffers;

public interface ICollectionOfferService
{
    Task<PaginatedResult<CollectionOfferOveralForCollectorModel>> GetCollectionOffersForCollector(int pageNumber,
        int pageSize, OfferStatus? offerStatus, bool? sortByCreateAt, Guid collectorId);

    Task<PaginatedResult<CollectionOfferOveralForHouseModel>> GetCollectionOffersForHousehold(int pageNumber,
        int pageSize, OfferStatus? offerStatus, Guid scrapPostId);

    Task<CollectionOfferModel> GetCollectionOffer(Guid scrapPostId, Guid collectionOfferId);

    Task<CollectionOfferModel> CreateCollectionOffer(Guid scrapPostId, Guid scrapCollectorId,
        CollectionOfferCreateModel model);

    Task RejectOrAcceptCollectionOffer(Guid collectionOfferId, Guid scrapPostId, Guid householdId, bool isAccepted);
    Task CancelOrReopenCollectionOffer(Guid collectionOfferId, Guid collectorId);
}