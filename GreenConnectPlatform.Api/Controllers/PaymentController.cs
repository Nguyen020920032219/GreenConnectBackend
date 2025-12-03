using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Payment;
using GreenConnectPlatform.Business.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/payment")]
[ApiController]
[Tags("20. Payment (Thanh Toán)")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    ///     (Collector) Tạo link thanh toán VNPay.
    /// </summary>
    [HttpPost("create-url")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    public async Task<IActionResult> CreateUrl([FromBody] CreatePaymentRequest request)
    {
        var userId = GetCurrentUserId();
        var res = await _paymentService.CreatePaymentLinkAsync(userId, request, HttpContext);
        return Ok(res);
    }

    /// <summary>
    ///     (User Browser) Return URL - Điều hướng về App.
    /// </summary>
    [HttpGet("return")]
    [AllowAnonymous]
    public IActionResult PaymentReturn()
    {
        try
        {
            var responseCode = Request.Query["vnp_ResponseCode"];
            var status = responseCode == "00" ? "success" : "failed";
            var redirectUrl = $"greenconnect://payment-result?status={status}";

            return Redirect(redirectUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    /// <summary>
    ///     (VNPay Server) IPN - Xử lý cộng tiền.
    /// </summary>
    [HttpGet("ipn")]
    [AllowAnonymous]
    public async Task<IActionResult> PaymentIpn()
    {
        try
        {
            await _paymentService.ProcessPaymentCallbackAsync(HttpContext.Request.Query);
            return Ok(new { RspCode = "00", Message = "Confirm Success" });
        }
        catch (ApiExceptionModel ex)
        {
            return Ok(new { RspCode = "99", ex.Message });
        }
        catch (Exception)
        {
            return Ok(new { RspCode = "99", Message = "Unknown Error" });
        }
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}