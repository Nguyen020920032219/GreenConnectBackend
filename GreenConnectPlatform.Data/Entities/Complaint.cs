using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class Complaint
{
    public Guid ComplaintId { get; set; }
    public string Reason { get; set; } = null!;
    public string? EvidenceUrl { get; set; }
    public ComplaintStatus Status { get; set; } = ComplaintStatus.Submitted;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid TransactionId { get; set; }
    public Guid ComplainantId { get; set; }
    public Guid AccusedId { get; set; }
    public virtual Transaction Transaction { get; set; } = null!;
    public virtual User Complainant { get; set; } = null!;
    public virtual User Accused { get; set; } = null!;
}