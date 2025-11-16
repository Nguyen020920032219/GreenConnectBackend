using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.ScheduleProposals;

public interface IScheduleProposalService
{
    Task<PaginatedResult<ScheduleProposalModel>> GetScheduleProposalsByCollectionOfferId(int pageNumber, int pageSize,
        ProposalStatus? proposalStatus, bool? sortByCreateAt, Guid collectionOfferId);

    Task<PaginatedResult<ScheduleProposalModel>> GetScheduleProposalsByCollectorId(int pageNumber, int pageSize,
        ProposalStatus? proposalStatus, bool? sortByCreateAt, Guid collectorId);

    Task<ScheduleProposalModel> GetScheduleProposal(Guid collectionOfferId, Guid scheduleProposalId);

    Task<ScheduleProposalModel> ReScheduleProposal(Guid collectionOfferId, Guid userId,
        ScheduleProposalCreateModel model);

    Task<ScheduleProposalModel> UpdateScheduleProposal(Guid scrapCollectorId, Guid scheduleProposalId,
        DateTime? proposedTime, string? responseMessage);

    Task RejectOrAcceptScheduleProposal(Guid scheduleProposalId, Guid collectionOfferId, Guid householdId,
        bool isAccepted);

    Task CancelOrReopenScheduleProposal(Guid scheduleProposalId, Guid collectorId);
}