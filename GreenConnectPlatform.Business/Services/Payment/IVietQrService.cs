namespace GreenConnectPlatform.Business.Services.Payment;

public interface IVietQrService
{
    string GenerateQrUrl(string bankCode, string accountNo, string accountName, decimal amount, string content);
}