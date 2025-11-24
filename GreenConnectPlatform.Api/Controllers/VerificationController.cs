using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.VerificationInfos;
using GreenConnectPlatform.Business.Services.VerificationInfos;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("/api/v1/admin/verifications")]
[Tags("10. Verification Infos (Đơn xác minh người thu gom)")]
public class VerificationController(IVerificationInfoService verificationInfoService) : ControllerBase
{
    /// <summary>
    ///     (Admin) có thể lấy tất các đơn xác minh người thu gom với phân trang và sắp xếp.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortBySubmittedAt">Admin có thể sắp xếp theo ngày gửi đơn</param>
    /// <param name="sortByStatus">Admin có thể sắp xếp theo trạng thái của đơn</param>
    /// <response code="200">Danh sách các đơn xác minh người thu gom</response>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaginatedResult<VerificationInfoOveralModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetVerifications(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] bool sortBySubmittedAt = true, 
        [FromQuery] VerificationStatus? sortByStatus = null)
    {
        var result = await verificationInfoService.GetVerificationInfos(pageNumber, pageSize, sortBySubmittedAt, sortByStatus);
        return Ok(result);
    }
    
    /// <summary>
    ///     Admin có thể lấy chi tiết đơn xác minh người thu gom bằng userId.
    /// </summary>
    /// <param name="userId">Id của người dùng mà admin muốn lấy</param>
    /// <response code="200">Trả về đơn xác minh người thu gom</response>
    /// <response code="404">Không tìm thấy đơn xác minh nào</response>
    [HttpGet("{userId:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(VerificationInfoModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVerificationByUserId([FromRoute] Guid userId)
    {
        var result =  await verificationInfoService.GetVerificationInfo(userId);
        return Ok(result);
    }
    
    /// <summary>
    ///  Admin có thể chấp nhận hoặc từ chối đơn xác minh người thu gom
    /// </summary>
    /// <param name="userId">Id của user</param>
    /// <param name="isAccepted">Nếu đông ý là True và từ chối là False</param>
    /// <param name="reviewerNote">Lời nhắn từ Admin khi xử lí xong đơn</param>
    /// <returns></returns>
    [HttpPatch("{userId:Guid}/status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVerificationStatus(
        [FromRoute] Guid userId, 
        [FromQuery] bool isAccepted,
        [FromQuery] string? reviewerNote)
    {
        var collectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await verificationInfoService.VerifyCollector(userId, Guid.Parse(collectorId), isAccepted, reviewerNote);
        return Ok(isAccepted ? "Verification accepted" : "Verification rejected");
    }
    
    /// <summary>
    ///     (InvidualCollector/BusinessCollector) có thể cập nhật thông tin đơn xác minh của mình
    /// </summary>
    /// <param name="documentFrontUrl"></param>
    /// <param name="documentBackUrl"></param>
    /// <returns></returns>
    [HttpPatch("update-verification")]
    [Authorize]
    [ProducesResponseType(typeof(VerificationInfoModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVerificationInfo(
        [FromQuery] string? documentFrontUrl,
        [FromQuery] string? documentBackUrl,
        [FromQuery] BuyerType? buyerType)
    {
        var collectorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await verificationInfoService.UpdateVerificationInfo(Guid.Parse(collectorId),buyerType, documentFrontUrl, documentBackUrl);
        return Ok(result);
    }
    
}