using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.ScheduleProposals;

public interface IScheduleProposalRepository : IBaseRepository<ScheduleProposal, Guid>
{
    Task<ScheduleProposal?> GetByIdWithDetailsAsync(Guid id);

    Task<(List<ScheduleProposal> Items, int TotalCount)> GetByOfferAsync(
        Guid offerId,
        ProposalStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize);

    Task<(List<ScheduleProposal> Items, int TotalCount)> GetByCollectorAsync(
        Guid collectorId,
        ProposalStatus? status,
        bool sortByCreateAtDesc,
        int pageIndex,
        int pageSize);
    
    Task<List<ScheduleProposal>> GetByOffer(Guid offerId);
}