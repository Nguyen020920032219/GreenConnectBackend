namespace GreenConnectPlatform.Business.Models.Payment;

public class PaymentLinkResponse
{
    public string PaymentUrl { get; set; } = string.Empty;
    public string TransactionRef { get; set; } = string.Empty;
}