using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScheduleProposals;

public class ScheduleProposalRepository : BaseRepository<GreenConnectDbContext, ScheduleProposal, Guid>, IScheduleProposalRepository
{
    public ScheduleProposalRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }
}