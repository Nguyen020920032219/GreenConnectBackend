using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.ScheduleProposals;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.ScheduleProposals;

public class ScheduleProposalService : IScheduleProposalService
{
    private readonly IMapper _mapper;
    private readonly ICollectionOfferRepository _offerRepository;
    private readonly IScheduleProposalRepository _proposalRepository;

    public ScheduleProposalService(
        IScheduleProposalRepository proposalRepository,
        ICollectionOfferRepository offerRepository,
        IMapper mapper)
    {
        _proposalRepository = proposalRepository;
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ScheduleProposalModel>> GetByOfferAsync(
        int pageNumber, int pageSize, ProposalStatus? status, bool sortByCreateAtDesc, Guid offerId)
    {
        var (items, total) =
            await _proposalRepository.GetByOfferAsync(offerId, status, sortByCreateAtDesc, pageNumber, pageSize);
        var data = _mapper.Map<List<ScheduleProposalModel>>(items);
        return new PaginatedResult<ScheduleProposalModel>
            { Data = data, Pagination = new PaginationModel(total, pageNumber, pageSize) };
    }

    public async Task<PaginatedResult<ScheduleProposalModel>> GetByCollectorAsync(
        int pageNumber, int pageSize, ProposalStatus? status, bool sortByCreateAtDesc, Guid collectorId)
    {
        var (items, total) =
            await _proposalRepository.GetByCollectorAsync(collectorId, status, sortByCreateAtDesc, pageNumber,
                pageSize);
        var data = _mapper.Map<List<ScheduleProposalModel>>(items);
        return new PaginatedResult<ScheduleProposalModel>
            { Data = data, Pagination = new PaginationModel(total, pageNumber, pageSize) };
    }

    public async Task<ScheduleProposalModel> GetByIdAsync(Guid id)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(id);
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal not found.");
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task<ScheduleProposalModel> CreateAsync(Guid collectorId, Guid offerId,
        ScheduleProposalCreateModel request)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Offer not found.");

        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (offer.Status != OfferStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Can only reschedule for Pending offers.");

        var proposal = _mapper.Map<ScheduleProposal>(request);
        proposal.ScheduleProposalId = Guid.NewGuid();
        proposal.CollectionOfferId = offerId;
        proposal.ProposerId = collectorId;
        proposal.Status = ProposalStatus.Pending;
        proposal.CreatedAt = DateTime.UtcNow;

        await _proposalRepository.AddAsync(proposal);
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task<ScheduleProposalModel> UpdateAsync(Guid collectorId, Guid proposalId, DateTime? proposedTime,
        string? message)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(proposalId);
        if (proposal == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Proposal not found.");

        if (proposal.ProposerId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (proposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot update accepted proposal.");

        if (proposedTime.HasValue) proposal.ProposedTime = proposedTime.Value;
        if (!string.IsNullOrEmpty(message)) proposal.ResponseMessage = message;

        await _proposalRepository.UpdateAsync(proposal);
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task ToggleCancelAsync(Guid collectorId, Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(proposalId);
        if (proposal == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Proposal not found.");

        if (proposal.ProposerId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Not authorized.");

        if (proposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Cannot cancel accepted proposal.");

        if (proposal.Status == ProposalStatus.Pending) proposal.Status = ProposalStatus.Canceled;
        else if (proposal.Status == ProposalStatus.Canceled) proposal.Status = ProposalStatus.Pending;
        else
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Can only toggle between Pending and Canceled.");

        await _proposalRepository.UpdateAsync(proposal);
    }

    public async Task ProcessProposalAsync(Guid householdId, Guid proposalId, bool isAccepted)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(proposalId);
        if (proposal == null) throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Proposal not found.");

        if (proposal.Offer.ScrapPost.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Only the post owner can accept/reject schedule.");

        if (proposal.Status != ProposalStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Can only process Pending proposals.");

        if (isAccepted)
            proposal.Status = ProposalStatus.Accepted;
        else
            proposal.Status = ProposalStatus.Rejected;

        await _proposalRepository.UpdateAsync(proposal);
    }
}