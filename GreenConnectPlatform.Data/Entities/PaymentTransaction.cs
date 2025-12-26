using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Data.Entities;

public class PaymentTransaction
{
    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public Guid? PackageId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentGateway { get; set; } = "VNPay";
    public PaymentStatus Status { get; set; }
    public string? TransactionRef { get; set; }
    public string? VnpTransactionNo { get; set; }
    public string? BankCode { get; set; }
    public string? ResponseCode { get; set; }
    public string? OrderInfo { get; set; }
    public string? ClientIpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? User { get; set; }
    public virtual PaymentPackage? Package { get; set; }
}