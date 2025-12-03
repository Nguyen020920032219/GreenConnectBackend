using System.Web;

namespace GreenConnectPlatform.Business.Services.Payment;

public class VietQrService : IVietQrService
{
    public string GenerateQrUrl(string bankCode, string accountNo, string accountName, decimal amount, string content)
    {
        // Xử lý tên chủ tài khoản (Thay khoảng trắng bằng %20 - UrlEncode)
        var encodedName = HttpUtility.UrlEncode(accountName);
        var encodedContent = HttpUtility.UrlEncode(content);

        // Template "compact2" là mẫu gọn đẹp, phù hợp mobile
        var url =
            $"https://img.vietqr.io/image/{bankCode}-{accountNo}-compact2.png?amount={amount}&addInfo={encodedContent}&accountName={encodedName}";

        return url;
    }
}