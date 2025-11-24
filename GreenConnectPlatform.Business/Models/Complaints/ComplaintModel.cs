using GreenConnectPlatform.Business.Models.Transactions;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.Complaints;

public class ComplaintModel
{
    public Guid ComplaintId { get; set; }

    public Guid TransactionId { get; set; }
    public TransactionModel Transaction { get; set; } = null!;

    public Guid ComplainantId { get; set; }
    public UserViewModel Complainant { get; set; } = null!;

    public Guid AccusedId { get; set; }
    public UserViewModel Accused { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public string? EvidenceUrl { get; set; }

    public ComplaintStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}