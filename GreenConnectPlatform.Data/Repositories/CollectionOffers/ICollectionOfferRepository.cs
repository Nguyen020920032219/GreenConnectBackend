using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CollectionOffers;

public interface ICollectionOfferRepository : IBaseRepository<CollectionOffer, Guid>
{
    Task<CollectionOffer?> GetByIdWithDetailsAsync(Guid id);

    Task<(List<CollectionOffer> Items, int TotalCount)> GetByCollectorAsync(
        Guid collectorId,
        OfferStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize);

    Task<(List<CollectionOffer> Items, int TotalCount)> GetByPostIdAsync(
        Guid postId,
        OfferStatus? status,
        int pageIndex,
        int pageSize);

    Task<List<CollectionOffer>> GetOffersForReport(Guid userId, DateTime startDate, DateTime endDate);
}