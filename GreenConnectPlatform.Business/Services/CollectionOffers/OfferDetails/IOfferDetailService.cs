using GreenConnectPlatform.Business.Models.CollectionOffers.OfferDetails;

namespace GreenConnectPlatform.Business.Services.CollectionOffers.OfferDetails;

public interface IOfferDetailService
{
    Task<OfferDetailModel> GetOfferDetail(Guid offerDetailId, Guid offerId);

    Task<OfferDetailModel> AddOfferDetail(Guid collectorId, Guid collectionOfferId,
        OfferDetailCreateModel offerDetailCreateModel);

    Task<OfferDetailModel> UpdateOfferDetail(Guid collectorId, Guid offerDetailId,
        OfferDetailUpdateModel offerDetailUpdateModel);

    Task DeleteOfferDetail(Guid collectorId, Guid offerDetailId);
}