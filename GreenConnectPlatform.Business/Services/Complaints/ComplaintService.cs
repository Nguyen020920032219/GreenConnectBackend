using AutoMapper;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Complaints;
using GreenConnectPlatform.Data.Repositories.PointHistories;
using GreenConnectPlatform.Data.Repositories.Transactions;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Complaints;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;
    private readonly IPointHistoryRepository _pointHistoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public ComplaintService(IComplaintRepository complaintRepository, ITransactionRepository transactionRepository,
        IFileStorageService fileStorageService, IPointHistoryRepository pointHistoryRepository, IMapper mapper)
    {
        _complaintRepository = complaintRepository;
        _transactionRepository = transactionRepository;
        _fileStorageService = fileStorageService;
        _pointHistoryRepository = pointHistoryRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<ComplaintModel>> GetComplaints(int pageNumber, int pageSize, bool sortByCreatedAt,
        ComplaintStatus? sortByStatus, Guid? userId, string? roleName)
    {
        var (items, totalRecords) = await _complaintRepository.GetComplaintsAsync(
            pageNumber,
            pageSize,
            sortByCreatedAt,
            sortByStatus,
            userId,
            roleName);
        var complaintModels = _mapper.Map<List<ComplaintModel>>(items);
        foreach (var complaintModel in complaintModels)
            if (!string.IsNullOrEmpty(complaintModel.EvidenceUrl))
                complaintModel.EvidenceUrl =
                    await _fileStorageService.GetReadSignedUrlAsync(complaintModel.EvidenceUrl);

        return new PaginatedResult<ComplaintModel>
        {
            Data = complaintModels,
            Pagination = new PaginationModel(totalRecords, pageNumber, pageSize)
        };
    }

    public async Task<ComplaintModel> GetComplaint(Guid complaintId)
    {
        var complaintTask = await _complaintRepository.GetComplaintByIdAsync(complaintId);
        if (complaintTask == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại không tồn tại");
        var complaintModel = _mapper.Map<ComplaintModel>(complaintTask);
        if (!string.IsNullOrEmpty(complaintModel.EvidenceUrl))
            complaintModel.EvidenceUrl = await _fileStorageService.GetReadSignedUrlAsync(complaintModel.EvidenceUrl);
        return complaintModel;
    }

    public async Task ProcessComplaintAsync(Guid complaintId, bool isAccept)
    {
        var complaintTask = await _complaintRepository.GetComplaintByIdAsync(complaintId);
        if (complaintTask == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại không tồn tại");
        if (complaintTask.Status != ComplaintStatus.InReview && complaintTask.Status != ComplaintStatus.Submitted)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Khiếu nại đã được xử lý");
        if (isAccept)
        {
            complaintTask.Status = ComplaintStatus.Resolved;
            complaintTask.Complainant.Profile.PointBalance += 30;
            await AddPointHistory(complaintTask.ComplainantId, 30, "Được hoàn và cộng điểm do khiếu nại thành công");
            complaintTask.Accused.Profile.PointBalance -= 20;
            await AddPointHistory(complaintTask.AccusedId, -20, "Bị trừ điểm do bị khiếu nại từ người dùng");
        }
        else
        {
            complaintTask.Status = ComplaintStatus.Dismissed;
        }

        await _complaintRepository.UpdateAsync(complaintTask);
    }

    public async Task<ComplaintModel> CreateComplaint(Guid userId, string roleName, ComplaintCreateModel model)
    {
        var transaction = await _transactionRepository.GetByIdWithDetailsAsync(model.TransactionId);
        if (transaction == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "400", "TransactionId này không tồn tại");
        if (transaction.Status == TransactionStatus.Scheduled || transaction.Status == TransactionStatus.InProgress)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể tạo phàn nàn khi giao dịch đang xử lí");
        if (roleName == "Household")
        {
            if (transaction.HouseholdId != userId)
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                    "Bạn không thuộc giao dịch này nên không không có quyền phàn nàn");
            if (transaction.Household.Profile.PointBalance < 20)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Bạn không đủ điểm để làm phàn nàn(cần 20 điểm để có thể làm phàn nàn)");
        }
        else
        {
            if (transaction.ScrapCollectorId != userId)
                throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                    "Bạn không thuộc giao dịch này nên không không có quyền phàn nàn");
            if (transaction.ScrapCollector.Profile.PointBalance < 20)
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Bạn không đủ điểm để làm phàn nàn(cần 20 điểm để có thể làm phàn nàn)");
        }

        var complaintModel = _mapper.Map<Complaint>(model);
        complaintModel.Status = ComplaintStatus.Submitted;
        complaintModel.CreatedAt = DateTime.Now;
        complaintModel.ComplainantId = userId;
        complaintModel.ComplaintId = Guid.NewGuid();
        if (roleName == "Household")
            complaintModel.AccusedId = transaction.ScrapCollectorId;
        else
            complaintModel.AccusedId = transaction.HouseholdId;
        await _complaintRepository.AddAsync(complaintModel);
        if (roleName == "Household")
        {
            transaction.Household.Profile.PointBalance -= 20;
            await AddPointHistory(transaction.HouseholdId, -20, "Bị trừ điểm do tạo phàn nàn");
        }
        else
        {
            transaction.ScrapCollector.Profile.PointBalance -= 20;
            await AddPointHistory(transaction.ScrapCollectorId, -20, "Bị trừ điểm do tạo phàn nàn");
        }

        await _transactionRepository.UpdateAsync(transaction);
        return _mapper.Map<ComplaintModel>(complaintModel);
    }

    public async Task<ComplaintModel> UpdateComplaint(Guid complaintId, Guid userId, string? reason,
        string? evidenceUrl)
    {
        var complaint = await _complaintRepository.GetComplaintByIdAsync(complaintId);
        if (complaint == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại này không tại");
        if (complaint.Status == ComplaintStatus.Resolved)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Không thể cập nhật thông tin khi đã hoàn thành");
        if (complaint.ComplainantId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không phải người tạo phàn nàn này");
        if (!string.IsNullOrWhiteSpace(reason))
            complaint.Reason = reason;
        if (!string.IsNullOrWhiteSpace(evidenceUrl))
            complaint.EvidenceUrl = evidenceUrl;
        await _complaintRepository.UpdateAsync(complaint);
        return _mapper.Map<ComplaintModel>(complaint);
    }

    public async Task ReopenComplaint(Guid complaintId, Guid userId)
    {
        var complaint = await _complaintRepository.GetComplaintByIdAsync(complaintId);
        if (complaint == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại này không tại");
        if (complaint.Status != ComplaintStatus.Dismissed)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Chỉ có thể mở lại phàn nàn khi bị bác bỏ");
        if (complaint.ComplainantId != userId)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Bạn không phải người tạo phàn nàn này");
        if (complaint.Complainant.Profile.PointBalance < 20)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                "Bạn không đủ điểm để làm phàn nàn(cần 20 điểm để có thể làm phàn nàn)");
        complaint.Status = ComplaintStatus.Submitted;
        complaint.Complainant.Profile.PointBalance -= 20;
        await AddPointHistory(complaint.ComplainantId, -20, "Bị trừ điểm do mở lại phàn nàn");
        await _complaintRepository.UpdateAsync(complaint);
    }

    private async Task AddPointHistory(Guid userId, int pointChange, string reason)
    {
        var pointHistory = new PointHistory
        {
            PointHistoryId = Guid.NewGuid(),
            UserId = userId,
            PointChange = pointChange,
            Reason = reason,
            CreatedAt = DateTime.Now
        };
        await _pointHistoryRepository.AddAsync(pointHistory);
    }
}