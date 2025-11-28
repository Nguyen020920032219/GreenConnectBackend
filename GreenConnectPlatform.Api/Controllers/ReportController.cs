using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Reports;
using GreenConnectPlatform.Business.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Tags("16. Report")]
public class ReportController(IReportService reportService) : ControllerBase
{
    /// <summary>
    ///     Admin có thể xem báo cáo tổng hợp trong khoảng thời gian
    /// </summary>
    /// <param name="start">Thời gian bắt đầu</param>
    /// <param name="end">Thời gian kết thúc</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ReportModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReport([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        else if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();

        if (end.Kind == DateTimeKind.Unspecified)
            end = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        else if (end.Kind == DateTimeKind.Local)
            end = end.ToUniversalTime();
        return Ok(await reportService.GetReport(start, end));
    }

    /// <summary>
    ///     Collector có thể xem báo cáo tổng hợp trong khoảng thời gian
    /// </summary>
    /// <param name="start">Thời gian bắt đầu</param>
    /// <param name="end">Thời gian kết thúc</param>
    /// <returns></returns>
    [HttpGet("collector")]
    [Authorize(Roles = "IndividualCollector,BusinessCollector")]
    [ProducesResponseType(typeof(ReportForCollectorModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetReportForCollector([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        if (start.Kind == DateTimeKind.Unspecified)
            start = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        else if (start.Kind == DateTimeKind.Local)
            start = start.ToUniversalTime();

        if (end.Kind == DateTimeKind.Unspecified)
            end = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        else if (end.Kind == DateTimeKind.Local)
            end = end.ToUniversalTime();

        var userId = GetCurrentUserId();
        return Ok(await reportService.GetReportForCollector(userId, start, end));
    }

    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}