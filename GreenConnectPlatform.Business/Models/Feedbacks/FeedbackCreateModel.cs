using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Feedbacks;

public class FeedbackCreateModel
{
    [Required(ErrorMessage = "TransactionId là bắt buộc")]
    public Guid TransactionId { get; set; }
    [Required(ErrorMessage = "Rate là bắt buộc")]
    [Range(1, 5, ErrorMessage = "Bạn chỉ có thể rating từ 1 - 5 thôi")]
    public int Rate { get; set; }
    
    public string? Comment { get; set; }
}