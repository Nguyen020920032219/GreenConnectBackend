using System.Security.Claims;
using GreenConnectPlatform.Business.Models.CreditTransactionHistories;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.CreditTransactionHistories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/credit-transaction")]
[Tags("22. Credit Transaction History")]
[ApiController]
public class CreditTransactionHistoryController(ICreditTransactionHistoryService creditTransactionHistoryService)
    : ControllerBase
{
    /// <summary>
    ///     (Collector) Xem danh sách lịch sử giao dịch tín dụng của mình.
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreatedAt">Sắp xếp theo ngày đã tạo ra giao dịch</param>
    /// <param name="type">Lọc theo loại giao dịch(Purchase, Usage, Refund, Bonus)</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "IndividualCollector, BusinessCollector")]
    [ProducesResponseType(typeof(PaginatedResult<CreditTransactionHistoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCreditTransactionHistories(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatedAt = true,
        [FromQuery] string? type = null)
    {
        var userId = GetCurrentUserId();
        var result =
            await creditTransactionHistoryService.GetCreditTransactionHistoriesByUserIdAsync(pageIndex, pageSize,
                userId, sortByCreatedAt, type);
        return Ok(result);
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}