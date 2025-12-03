using System.ComponentModel.DataAnnotations;

namespace GreenConnectPlatform.Business.Models.Payment;

public class CreatePaymentRequest
{
    [Required] public Guid PackageId { get; set; }
}