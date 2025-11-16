using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CollectionOffers;

public class CollectionOfferRepository : BaseRepository<GreenConnectDbContext, CollectionOffer, Guid>,
    ICollectionOfferRepository
{
    public CollectionOfferRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}