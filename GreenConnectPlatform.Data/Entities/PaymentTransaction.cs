using System.ComponentModel.DataAnnotations.Schema;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class PaymentTransaction
{
    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public Guid? PackageId { get; set; }
    public decimal Amount { get; set; } 
    public string PaymentGateway { get; set; } = "VNPay"; 
    public string? TransactionCode { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
    public virtual PaymentPackage? Package { get; set; }
}