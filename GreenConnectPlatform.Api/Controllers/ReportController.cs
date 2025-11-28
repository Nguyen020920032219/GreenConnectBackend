using GreenConnectPlatform.Business.Services.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Tags("16. Report")]
public class ReportController(IReportService reportService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
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
}