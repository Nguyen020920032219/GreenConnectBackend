using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.CollectorVerificationInfos;

public class VerificationInfoRepository : BaseRepository<GreenConnectDbContext, CollectorVerificationInfo, Guid>,
    IVerificationInfoRepository
{
    public VerificationInfoRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}