using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Complaints;

public interface IComplaintRepository : IBaseRepository<Complaint, Guid>
{
    Task<Complaint?> GetComplaintByIdAsync(Guid complaintId);

    Task<(List<Complaint> Items, int TotalCount)> GetComplaintsAsync(
        int pageIndex,
        int pageSize,
        bool sortByCreatedAt,
        ComplaintStatus? sortByStatus,
        Guid? userId,
        string? roleName);

    Task<List<Complaint>> GetComplaintsForReport(DateTime startDate, DateTime endDate);
}