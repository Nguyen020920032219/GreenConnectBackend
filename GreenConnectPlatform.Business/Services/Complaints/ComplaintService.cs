using AutoMapper;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Complaints;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Complaints;

public class ComplaintService : IComplaintService
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IMapper _mapper;
    
    public ComplaintService(IComplaintRepository complaintRepository, IMapper mapper)
    {
        _complaintRepository = complaintRepository;
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
        return _mapper.Map<ComplaintModel>(complaintTask);
    }

    public async Task ProcessComplaintAsync(Guid complaintId, bool isAccept)
    {
        var complaintTask = await _complaintRepository.GetComplaintByIdAsync(complaintId);
        if (complaintTask == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Khiếu nại không tồn tại");
        if(complaintTask.Status != ComplaintStatus.InReview)
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", "Khiếu nại đã được xử lý");
        if (isAccept)
        {
            complaintTask.Status = ComplaintStatus.Resolved;
            complaintTask.Complainant.Profile.PointBalance += 30;
            complaintTask.Accused.Profile.PointBalance -= 20;
        }
        else
            complaintTask.Status = ComplaintStatus.Dismissed;
        await _complaintRepository.UpdateAsync(complaintTask);
    }
}