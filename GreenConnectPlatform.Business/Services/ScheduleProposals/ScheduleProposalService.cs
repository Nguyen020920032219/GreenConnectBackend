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
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Lịch hẹn trước không tìm thấy.");
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task<ScheduleProposalModel> CreateAsync(Guid collectorId, Guid offerId,
        ScheduleProposalCreateModel request)
    {
        var offer = await _offerRepository.GetByIdWithDetailsAsync(offerId);
        if (offer == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Đề nghị này không tìm thấy.");

        if (offer.ScrapCollectorId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (offer.Status != OfferStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể gửi lại lịch hẹn khi đề nghị không ở trạng thái pending.");

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
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Lich hẹn trước không tìm thấy.");

        if (proposal.ProposerId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (proposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể cập nhật khi đã được chấp nhận.");

        if (proposedTime.HasValue) proposal.ProposedTime = proposedTime.Value;
        if (!string.IsNullOrEmpty(message)) proposal.ResponseMessage = message;

        await _proposalRepository.UpdateAsync(proposal);
        return _mapper.Map<ScheduleProposalModel>(proposal);
    }

    public async Task ToggleCancelAsync(Guid collectorId, Guid proposalId)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(proposalId);
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Lịch hẹn trước không tìm thấy.");

        if (proposal.ProposerId != collectorId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không có quyền.");

        if (proposal.Status == ProposalStatus.Accepted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không thể hủy khi đã được chấp nhận.");

        if (proposal.Status == ProposalStatus.Canceled || proposal.Status == ProposalStatus.Rejected)
            if (proposal.Offer.Status == OfferStatus.Pending)
                proposal.Status = ProposalStatus.Pending;
            else
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Bạn chỉ có thể mở lại đề xuất lịch trình nếu đề nghị thu thập liên quan vẫn đang chờ xử lý");

        else if (proposal.Status == ProposalStatus.Pending)
            proposal.Status = ProposalStatus.Canceled;

        await _proposalRepository.UpdateAsync(proposal);
    }

    public async Task ProcessProposalAsync(Guid householdId, Guid proposalId, bool isAccepted)
    {
        var proposal = await _proposalRepository.GetByIdWithDetailsAsync(proposalId);
        if (proposal == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Lịch hẹn trước không tìm thấy.");

        if (proposal.Offer.ScrapPost.HouseholdId != householdId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Chỉ chủ bài đăng mới có thể chấp nhận/từ chối lịch trình.");

        if (proposal.Status != ProposalStatus.Pending)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Chỉ có thể xử lý các đề xuất đang chờ xử lý.");

        if (isAccepted)
        {
            proposal.Status = ProposalStatus.Accepted;
            var schedules = await _proposalRepository.GetByOffer(proposal.CollectionOfferId);
            var scheduleIsPending = schedules
                .Where(s => s.Status == ProposalStatus.Pending && s.ScheduleProposalId != proposalId).ToList();
            foreach (var schedule in scheduleIsPending) schedule.Status = ProposalStatus.Canceled;
        }
        else
        {
            proposal.Status = ProposalStatus.Rejected;
        }

        await _proposalRepository.UpdateAsync(proposal);
    }
}