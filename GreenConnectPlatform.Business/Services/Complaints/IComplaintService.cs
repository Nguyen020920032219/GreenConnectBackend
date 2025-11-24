using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Services.Complaints;

public interface IComplaintService
{
    Task<PaginatedResult<ComplaintModel>> GetComplaints(int pageNumber, int pageSize, bool sortByCreatedAt, 
        ComplaintStatus? sortByStatus, Guid? userId, string? roleName);
    Task<ComplaintModel> GetComplaint(Guid complaintId);
    
    Task ProcessComplaintAsync(Guid complaintId, bool isAccept);
}