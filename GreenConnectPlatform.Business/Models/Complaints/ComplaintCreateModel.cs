using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Complaints;

public class ComplaintCreateModel
{
    [Required(ErrorMessage = "TransactionId là bắt buộc.")]
    public Guid TransactionId { get; set; }

    [Required(ErrorMessage = "Reason là bắt buộc.")]
    public string Reason { get; set; } = null!;

    public string? EvidenceUrl { get; set; }
}