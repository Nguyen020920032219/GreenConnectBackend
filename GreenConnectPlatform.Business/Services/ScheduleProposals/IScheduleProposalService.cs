using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.ScheduleProposals;

public interface IScheduleProposalService
{
    Task<PaginatedResult<ScheduleProposalModel>> GetByOfferAsync(
        int pageNumber, int pageSize, ProposalStatus? status, bool sortByCreateAtDesc, Guid offerId);

    Task<PaginatedResult<ScheduleProposalModel>> GetByCollectorAsync(
        int pageNumber, int pageSize, ProposalStatus? status, bool sortByCreateAtDesc, Guid collectorId);

    Task<ScheduleProposalModel> GetByIdAsync(Guid id);
    Task<ScheduleProposalModel> CreateAsync(Guid collectorId, Guid offerId, ScheduleProposalCreateModel request);
    Task<ScheduleProposalModel> UpdateAsync(Guid collectorId, Guid proposalId, DateTime? proposedTime, string? message);
    Task ToggleCancelAsync(Guid collectorId, Guid proposalId);
    Task ProcessProposalAsync(Guid householdId, Guid proposalId, bool isAccepted, string? responseMessage);
}