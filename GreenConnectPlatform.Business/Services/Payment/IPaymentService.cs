using GreenConnectPlatform.Business.Models.Payment;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Payment;

public interface IPaymentService
{
    Task<PaymentLinkResponse>
        CreatePaymentLinkAsync(Guid userId, CreatePaymentRequest request, HttpContext httpContext);

    Task ProcessPaymentCallbackAsync(IQueryCollection collections);
}