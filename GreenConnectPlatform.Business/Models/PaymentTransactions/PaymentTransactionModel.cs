using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;

namespace GreenConnectPlatform.Business.Models.PaymentTransactions;

public class PaymentTransactionModel
{
    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public UserViewModel User { get; set; } = new();
    public Guid? PackageId { get; set; }
    public PaymentPackageModel Package { get; set; } = new();
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
}