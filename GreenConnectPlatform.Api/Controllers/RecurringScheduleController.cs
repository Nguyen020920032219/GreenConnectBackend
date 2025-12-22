using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.RecurringScheduleDetails;
using GreenConnectPlatform.Business.Models.RecurringSchedules;
using GreenConnectPlatform.Business.Services.RecurringSchedules;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;
[ApiController]
[Route("api/v1/recurring-schedules")]
[Tags("24. Recurring Schedules")]
public class RecurringScheduleController(IRecurringScheduleService service) : ControllerBase
{
    /// <summary>
    ///  Household có thể xem danh sách các lịch trình định kỳ đã tạo với chức năng phân trang và sắp xếp theo ngày tạo.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByCreatedAt"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleOverallModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPagedRecurringSchedules([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, 
        [FromQuery] bool sortByCreatedAt = true)
    {
        var result = await service.GetPagedRecurringSchedulesAsync(pageNumber, pageSize, sortByCreatedAt);
        return Ok(result);
    }
    /// <summary>
    ///  Household có thể xem chi tiết một lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecurringScheduleById([FromRoute] Guid id)
    {
        var result = await service.GetRecurringScheduleByIdAsync(id);
        return Ok(result);
    }
    /// <summary>
    ///  Household có thể tạo một lịch trình định kỳ mới bằng cách cung cấp các chi tiết cần thiết.
    /// </summary>
    /// <param name="model"></param>
    /// <remarks>
    ///  Đối với DayOfWeek sẽ là từ 0 - 6 tương ứng với 0 là Chủ nhật, 1 là thứ 2, 2 là thứ 3.......6 là thứ 7.
    /// </remarks>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleOverallModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRecurringSchedule([FromBody] RecurringScheduleCreateModel model)
    {
        var householdId = GetCurrentUserId();
        var result = await service.CreateRecurringScheduleAsync(householdId, model);
        return CreatedAtAction(nameof(GetRecurringScheduleById), new { id = result.Id }, result);
    }
    /// <summary>
    ///  Household có thể cập nhật các chi tiết của một lịch trình định kỳ hiện có bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleOverallModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRecurringSchedule([FromRoute]Guid id,[FromBody] RecurringScheduleUpdateModel model)
    {
        var householdId = GetCurrentUserId();
        var result = await service.UpdateRecurringScheduleAsync(id, householdId, model);
        return Ok(result);
    }
    /// <summary>
    ///  Household có thể bật hoặc tắt một lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleOverallModel), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ToggleRecurringSchedule([FromRoute] Guid id)
    {
        var householdId = GetCurrentUserId();
        await service.ToggleRecurringScheduleAsync(id, householdId);
        return NoContent();
    }

    /// <summary>
    ///  Household có thể xem chi tiết một chi tiết lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("details/{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecurringScheduleDetail([FromRoute] Guid id)
    {
        return Ok(await service.GetRecurringScheduleDetailAsync(id));
    }
    
    /// <summary>
    ///  Household có thể thêm một chi tiết lịch trình định kỳ mới vào một lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("{id:guid}/details")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleDetailModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRecurringScheduleDetail([FromRoute] Guid id, [FromBody] RecurringScheduleDetailCreateModel model)
    {
        var householdId = GetCurrentUserId();
        var result = await service.AddRecurringScheduleDetailAsync(id, householdId, model);
        return CreatedAtAction(nameof(GetRecurringScheduleDetail), new { id = result.Id }, result);
    }
    
    /// <summary>
    /// Household có thể cập nhật các chi tiết của một chi tiết lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="detailId"></param>
    /// <param name="quantity"></param>
    /// <param name="amountDescription"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    [HttpPatch("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleDetailModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRecurringScheduleDetail([FromRoute] Guid id, [FromRoute] Guid detailId,
        [FromQuery] double? quantity, [FromQuery] string? amountDescription, [FromQuery] ItemTransactionType? type)
    {
        var householdId = GetCurrentUserId();
        var result = await service.UpdateRecurringScheduleDetailAsync(id, detailId, householdId, quantity, amountDescription, type);
        return Ok(result);
    }

    /// <summary>
    ///  Household có thể xóa một chi tiết lịch trình định kỳ cụ thể bằng cách sử dụng ID của nó.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="detailId"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}/details/{detailId:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(RecurringScheduleDetailModel), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecurringScheduleDetail([FromRoute] Guid id, [FromRoute] Guid detailId)
    {
        var householdId = GetCurrentUserId();
        await service.DeleteRecurringScheduleDetailAsync(id, detailId, householdId);
        return NoContent();
    }
    
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}