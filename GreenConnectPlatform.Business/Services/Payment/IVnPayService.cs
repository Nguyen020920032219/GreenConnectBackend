using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Payment;

public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context, string txnRef, double amount, string orderInfo);
    VnPayResponseModel PaymentExecute(IQueryCollection collections);
}