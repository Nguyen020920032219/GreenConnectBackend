using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Feedbacks;
using GreenConnectPlatform.Business.Services.Feedbacks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/feedbacks")]
[Tags ("17. Feedback (Nhận xét)")]
public class FeedbackController(IFeedbackService feedbackService) : Controller
{
    /// <summary>
    ///     (Collector/Household) Xem danh sách đánh giá của tôi.
    /// </summary>
    /// <remarks>
    ///     Lấy danh sách các đánh giá liên quan đến người dùng hiện tại (được phân trang).
    ///     <br/>
    ///     - **Household**: Xem các đánh giá mình đã viết cho Collector.
    ///     - **Collector**: Xem các đánh giá mà mình nhận được từ Household.
    /// </remarks>
    /// <param name="pageNumber">Số trang hiện tại (Mặc định: 1).</param>
    /// <param name="pageSize">Số lượng đánh giá trên một trang (Mặc định: 10).</param>
    /// <param name="sortByCreatAt">`true`: Sắp xếp mới nhất, `false`: Cũ nhất.</param>
    /// <response code="200">Thành công. Trả về danh sách đánh giá.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền truy cập (Role không hợp lệ).</response>
    [HttpGet("my-feedbacks")]
    [Authorize(Roles = "IndividualCollector, BusinessCollector, Household")]
    [ProducesResponseType(typeof(FeedbackModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyFeedbacks([FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] bool sortByCreatAt = true)
    {
        var userId = GetCurrentUserId();
        var role = User.FindFirstValue(ClaimTypes.Role) ?? "";
        return Ok(await feedbackService.GetFeedbacksByUserIdAsync(pageNumber, pageSize, userId, role, sortByCreatAt));
    }

    /// <summary>
    ///     (All) Xem chi tiết một đánh giá.
    /// </summary>
    /// <remarks>
    ///     Lấy thông tin cụ thể của một feedback dựa trên ID.
    ///     Bao gồm: Nội dung comment, số sao đánh giá, thông tin người gửi và người nhận.
    /// </remarks>
    /// <param name="id">ID của đánh giá (Guid).</param>
    /// <response code="200">Thành công. Trả về chi tiết `FeedbackModel`.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="404">Không tìm thấy đánh giá.</response>
    [HttpGet("{id:Guid}")]
    [Authorize]
    [ProducesResponseType(typeof(FeedbackModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFeedbackById([FromRoute]Guid id)
    {
        return Ok(await feedbackService.GetFeedbackByIdAsync(id));
    }

    /// <summary>
    ///     (Household) Tạo đánh giá mới.
    /// </summary>
    /// <remarks>
    ///     Cho phép người dân (Household) đánh giá Collector sau khi hoàn thành giao dịch thu gom.
    ///     Yêu cầu: Phải có `TransactionId` hợp lệ và giao dịch đã hoàn tất.
    /// </remarks>
    /// <param name="feedback">Dữ liệu đánh giá (`TransactionId`, `Rate`, `Comment`...).</param>
    /// <response code="201">Tạo thành công. Trả về thông tin đánh giá vừa tạo.</response>
    /// <response code="400">Dữ liệu không hợp lệ hoặc Giao dịch chưa hoàn tất.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không phải là Household.</response>
    /// <response code="404">Không tìm thấy giao dịch hoặc người dùng liên quan.</response>
    [HttpPost]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(FeedbackModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFeedback(FeedbackCreateModel feedback)
    {
        var userId = GetCurrentUserId();
        var result = await feedbackService.CreateFeedbackAsync(userId, feedback);
        return CreatedAtAction(nameof(GetFeedbackById), new { id = result.FeedbackId }, result);
    }

    /// <summary>
    ///     (Household) Cập nhật đánh giá.
    /// </summary>
    /// <remarks>
    ///     Cho phép người dùng chỉnh sửa số sao hoặc nội dung bình luận của đánh giá đã gửi trước đó.
    ///     Chỉ người tạo đánh giá mới có quyền sửa.
    /// </remarks>
    /// <param name="id">ID của đánh giá cần sửa.</param>
    /// <param name="rate">Số sao đánh giá mới (Tùy chọn).</param>
    /// <param name="comment">Nội dung bình luận mới (Tùy chọn).</param>
    /// <response code="200">Cập nhật thành công.</response>
    /// <response code="400">Dữ liệu không hợp lệ.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền (Không phải là chủ sở hữu đánh giá).</response>
    /// <response code="404">Không tìm thấy đánh giá.</response>
    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(FeedbackModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateFeedback([FromRoute] Guid id, [FromQuery] int? rate,
        [FromQuery] string? comment)
    {
        var userId = GetCurrentUserId();
        var result = await feedbackService.UpdateFeedbackAsync(id, userId, rate, comment);
        return Ok(result);
    }

    /// <summary>
    ///     (Household) Xóa đánh giá.
    /// </summary>
    /// <remarks>
    ///     Xóa bỏ một đánh giá đã gửi. Chỉ người tạo đánh giá mới có quyền xóa.
    /// </remarks>
    /// <param name="id">ID của đánh giá cần xóa.</param>
    /// <response code="200">Xóa thành công.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền (Không phải là chủ sở hữu).</response>
    /// <response code="404">Không tìm thấy đánh giá.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Household")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFeedback([FromRoute] Guid id)
    {
        var userId = GetCurrentUserId();
        await feedbackService.DeleteFeedbackAsync(id, userId);
        return Ok("Xóa nhận xét thành công");
    }
    
    private Guid GetCurrentUserId()
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
    }
}