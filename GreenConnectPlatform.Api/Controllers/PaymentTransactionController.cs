using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentTransactions;
using GreenConnectPlatform.Business.Services.PaymentTransactions;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("payment-transaction")]
[Tags("Payment Transaction")]
[ApiController]
public class PaymentTransactionController(IPaymentTransactionService paymentTransactionService) : ControllerBase
{
    /// <summary>
    ///  (Collector) Xem danh sách giao dịch thanh toán của mình.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreatedAt">Sắp xếp theo ngày giao dịch</param>
    /// <param name="status">Lọc theo trạng thái của giao dịch</param>
    /// <returns></returns>
    [HttpGet("my-transactions")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<PaymentTransactionModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPaymentTransactionsByUser(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatedAt = true,
        [FromQuery] PaymentStatus? status = null)
    {
        var userId = GetCurrentUserId();
        var result = await paymentTransactionService.GetPaymentTransactionsByUserAsync(pageIndex, pageSize, userId, sortByCreatedAt, status);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResult<PaymentTransactionModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPaymentTransactions(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatedAt = true,
        [FromQuery] PaymentStatus? status = null)
    {
        if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        else if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();

        if (end.Kind == DateTimeKind.Unspecified)
            end = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        else if (end.Kind == DateTimeKind.Local)
            end = end.ToUniversalTime();
        var result = await paymentTransactionService.GetPaymentTransactionsAsync(pageIndex, pageSize, sortByCreatedAt, status, start, end);
        return Ok(result);
    }
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}