using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CollectionOffers.OfferDetails;

public class OfferDetailRepository : BaseRepository<GreenConnectDbContext, OfferDetail, Guid>, IOfferDetailRepository
{
    public OfferDetailRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}