using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.CollectionOffers;
using GreenConnectPlatform.Data.Repositories.ScheduleProposals;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Business.Services.ScheduleProposals;

public class ScheduleProposalService : IScheduleProposalService
{
    private readonly ICollectionOfferRepository _collectionOfferRepository;
    private readonly IMapper _mapper;
    private readonly IScheduleProposalRepository _scheduleProposalRepository;

    public ScheduleProposalService(IScheduleProposalRepository scheduleProposalRepository,
        ICollectionOfferRepository collectionOfferRepository, IMapper mapper)
    {
        _scheduleProposalRepository = scheduleProposalRepository;
        _collectionOfferRepository = collectionOfferRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ScheduleProposalModel>> GetScheduleProposalsByCollectionOfferId(int pageNumber,
        int pageSize, ProposalStatus? proposalStatus,
        bool? sortByCreateAt, Guid collectionOfferId)
    {
        var query = _scheduleProposalRepository.DbSet()
            .Where(x => x.CollectionOfferId == collectionOfferId)
            .AsQueryable()
            .AsNoTracking();
        if (proposalStatus != null)
            query = query.Where(x => x.Status == proposalStatus.Value);
        if (sortByCreateAt == true)
            query = query.OrderByDescending(c => c.CreatedAt);
        else
            query = query.OrderBy(c => c.CreatedAt);
        var totalRecords = await query.CountAsync();
        var scheduleProposals = await query
            .Include(s => s.Offer)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var scheduleModel = _mapper.Map<List<ScheduleProposalModel>>(scheduleProposals);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null
        };
        return new PaginatedResult<ScheduleProposalModel>
        {
            Data = scheduleModel,
            Pagination = paginationModel
        };
    }

    public async Task<PaginatedResult<ScheduleProposalModel>> GetScheduleProposalsByCollectorId(int pageNumber,
        int pageSize, ProposalStatus? proposalStatus,
        bool? sortByCreateAt, Guid collectorId)
    {
        var query = _scheduleProposalRepository.DbSet()
            .Where(x => x.ProposerId == collectorId)
            .AsQueryable()
            .AsNoTracking();
        if (proposalStatus != null)
            query = query.Where(x => x.Status == proposalStatus.Value);
        if (sortByCreateAt == true)
            query = query.OrderByDescending(c => c.CreatedAt);
        else
            query = query.OrderBy(c => c.CreatedAt);
        var totalRecords = await query.CountAsync();
        var scheduleProposals = await query
            .Include(s => s.Offer)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var scheduleModel = _mapper.Map<List<ScheduleProposalModel>>(scheduleProposals);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        var paginationModel = new PaginationModel
        {
            TotalRecords = totalRecords,
            CurrentPage = pageNumber,
            TotalPages = totalPages,
            NextPage = pageNumber < totalPages ? pageNumber + 1 : null,
            PrevPage = pageNumber > 1 ? pageNumber - 1 : null
        };
        return new PaginatedResult<ScheduleProposalModel>
        {
            Data = scheduleModel,
            Pagination = paginationModel
        };
    }

    public async Task<ScheduleProposalModel> GetScheduleProposal(Guid collectionOfferId, Guid scheduleProposalId)
    {
        var proposal = await _scheduleProposalRepository.DbSet()
            .Include(s => s.Offer)
            .FirstOrDefaultAsync(p =>
                p.CollectionOfferId == collectionOfferId && p.ScheduleProposalId == scheduleProposalId);
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal does not exist");
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task<ScheduleProposalModel> ReScheduleProposal(Guid collectionOfferId, Guid userId,
        ScheduleProposalCreateModel model)
    {
        var collectionOffer = await _scheduleProposalRepository.DbSet()
            .FirstOrDefaultAsync(c => c.CollectionOfferId == collectionOfferId);
        if (collectionOffer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal does not exist");
        if (collectionOffer.Status != ProposalStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can only create schedule proposal to pending collection offer");
        var proposal = _mapper.Map<ScheduleProposal>(model);
        proposal.ScheduleProposalId = Guid.NewGuid();
        proposal.CollectionOfferId = collectionOfferId;
        proposal.ProposerId = userId;
        proposal.Status = ProposalStatus.Pending;
        proposal.CreatedAt = DateTime.UtcNow;
        var result = await _scheduleProposalRepository.Add(proposal);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to create schedule proposal");
        return _mapper.Map<ScheduleProposalModel>(result);
    }

    public async Task<ScheduleProposalModel> UpdateScheduleProposal(Guid scrapCollectorId, Guid scheduleProposalId,
        DateTime? proposedTime,
        string? responseMessage)
    {
        var proposal = await _scheduleProposalRepository.DbSet()
            .FirstOrDefaultAsync(p => p.ScheduleProposalId == scheduleProposalId && p.ProposerId == scrapCollectorId);
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal does not exist");
        if (proposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can not update schedule proposal that has been accepted");
        if (proposedTime != null) proposal.ProposedTime = proposedTime.Value;
        // else proposal.ProposedTime = proposal.ProposedTime;
        if (responseMessage != null) proposal.ResponseMessage = responseMessage;
        // else proposal.ResponseMessage = proposal.ResponseMessage;
        var result = await _scheduleProposalRepository.Update(proposal);
        if (result == null)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Failed to update schedule proposal");
        return _mapper.Map<ScheduleProposalModel>(result);
    }

    public async Task RejectOrAcceptScheduleProposal(Guid scheduleProposalId, Guid collectionOfferId, Guid householdId,
        bool isAccepted)
    {
        var scheduleProposal = await _scheduleProposalRepository.DbSet()
            .Include(s => s.Offer)
            .ThenInclude(s => s.ScrapPost)
            .ThenInclude(s => s.Household)
            .FirstOrDefaultAsync(s =>
                s.ScheduleProposalId == scheduleProposalId && s.CollectionOfferId == collectionOfferId);
        if (scheduleProposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal does not exist");
        if (scheduleProposal.Offer.ScrapPost.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You can not reject or accept this schedule proposal");
        if (scheduleProposal.Status != ProposalStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can only reject or accept pending schedule proposal");
        if (isAccepted)
        {
            scheduleProposal.Status = ProposalStatus.Accepted;
            var scheduleIsPending = await _scheduleProposalRepository.DbSet()
                .Where(s => s.Status == ProposalStatus.Pending && s.CollectionOfferId == collectionOfferId &&
                            s.ScheduleProposalId != scheduleProposalId).ToListAsync();
            foreach (var schedule in scheduleIsPending) schedule.Status = ProposalStatus.Canceled;
        }
        else
        {
            scheduleProposal.Status = ProposalStatus.Rejected;
        }

        await _scheduleProposalRepository.Update(scheduleProposal);
    }

    public async Task CancelOrReopenScheduleProposal(Guid scheduleProposalId, Guid collectorId)
    {
        var scheduleProposal = await _scheduleProposalRepository.DbSet()
            .Include(s => s.Offer)
            .FirstOrDefaultAsync(s => s.ScheduleProposalId == scheduleProposalId);
        if (scheduleProposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Schedule proposal does not exist");
        if (scheduleProposal.ProposerId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "You can not cancel or reopen this schedule proposal");
        if (scheduleProposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "You can not cancel or reopen accepted schedule proposal");

        if (scheduleProposal.Status == ProposalStatus.Canceled || scheduleProposal.Status == ProposalStatus.Rejected)
            if (scheduleProposal.Offer.Status == OfferStatus.Pending)
                scheduleProposal.Status = ProposalStatus.Pending;
            else
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "You can only reopen schedule proposal if the related collection offer is still pending");

        else if (scheduleProposal.Status == ProposalStatus.Pending)
            scheduleProposal.Status = ProposalStatus.Canceled;

        await _scheduleProposalRepository.Update(scheduleProposal);
    }
}