using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentPackages;
using GreenConnectPlatform.Business.Services.PaymentPackages;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[Route("api/v1/packages")]
[Tags("13. Payment Packages (Gói thanh toán)")]
[Authorize]
public class PaymentPackageController(IPaymentPackageService packageService) : ControllerBase
{
    /// <summary>
    ///     User can get a list of payment packages with pagination, sorting and filtering options.
    ///     If the user is an Admin, they can see all packages including inactive ones.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortByPrice">True is sort ascending by price, False is sort deceasing by price</param>
    /// <param name="packageType">sort by Freemium or Paid</param>
    /// <param name="name">Search by name of payment package</param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<PaymentPackageOverallModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaymentPackages(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? sortByPrice = null,
        [FromQuery] PackageType? packageType = null,
        [FromQuery] string? name = null)
    {
        var roleName = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        var result =
            await packageService.GetPaymentPackages(pageNumber, pageSize, roleName, sortByPrice, packageType, name);
        return Ok(result);
    }

    /// <summary>
    ///     (All) Xem chi tiết gói nạp tiền.
    /// </summary>
    /// <remarks>
    ///     Lấy thông tin cụ thể của một gói nạp coin (Ví dụ: Gói Cơ bản, Gói VIP).
    ///     Trả về các thông tin: Tên gói, Giá tiền (VNĐ), Số coin nhận được, Mô tả...
    /// </remarks>
    /// <param name="packageId">ID của gói nạp cần xem (Guid).</param>
    /// <response code="200">Thành công. Trả về `PaymentPackageModel`.</response>
    /// <response code="401">Chưa đăng nhập (nếu hệ thống yêu cầu).</response>
    /// <response code="404">Không tìm thấy gói nạp với ID này.</response>
    [HttpGet("{packageId:Guid}")]
    [ProducesResponseType(typeof(PaymentPackageModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentPackage([FromRoute] Guid packageId)
    {
        var result = await packageService.GetPaymentPackage(packageId);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Tạo mới gói nạp tiền.
    /// </summary>
    /// <remarks>
    ///     Dành cho Quản trị viên tạo các gói nạp coin vào hệ thống.
    ///     Cần nhập đầy đủ: Tên gói, Giá tiền, Số coin quy đổi, Đơn vị tiền tệ...
    /// </remarks>
    /// <param name="model">Dữ liệu tạo gói nạp (`PaymentPackageCreateModel`).</param>
    /// <response code="201">Tạo thành công. Trả về thông tin gói vừa tạo.</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ (Thiếu trường bắt buộc, giá trị âm...).</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    /// <response code="500">Lỗi server nội bộ.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaymentPackageModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePaymentPackage([FromBody] PaymentPackageCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join("; ", errorMessages);
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", message);
        }

        var result = await packageService.CreatePaymentPackage(model);
        return CreatedAtAction(nameof(GetPaymentPackage), new { packageId = result.PackageId }, result);
    }

    /// <summary>
    ///     (Admin) Cập nhật thông tin gói nạp tiền.
    /// </summary>
    /// <remarks>
    ///     Cho phép Admin chỉnh sửa thông tin của gói nạp đã tồn tại (Ví dụ: Thay đổi giá tiền, Tên gói, Số coin khuyến
    ///     mãi...).
    /// </remarks>
    /// <param name="packageId">ID của gói nạp cần sửa.</param>
    /// <param name="model">Dữ liệu cần cập nhật (`PaymentPackageUpdateModel`).</param>
    /// <response code="200">Cập nhật thành công. Trả về model đã sửa.</response>
    /// <response code="400">Dữ liệu đầu vào không hợp lệ.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    /// <response code="404">Không tìm thấy gói nạp cần sửa.</response>
    /// <response code="500">Lỗi server nội bộ.</response>
    [HttpPatch("{packageId:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(PaymentPackageModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePaymentPackage([FromRoute] Guid packageId,
        [FromBody] PaymentPackageUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join("; ", errorMessages);
            throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400", message);
        }

        var result = await packageService.UpdatePaymentPackage(packageId, model);
        return Ok(result);
    }

    /// <summary>
    ///     (Admin) Ngưng hoạt động gói nạp tiền.
    /// </summary>
    /// <remarks>
    ///     Chuyển trạng thái gói nạp sang "Inactive" (hoặc xóa mềm).
    ///     Gói này sẽ không còn hiển thị cho người dùng mua nữa, nhưng vẫn lưu trong Database để đối soát lịch sử.
    /// </remarks>
    /// <param name="packageId">ID của gói nạp cần ngưng hoạt động.</param>
    /// <response code="200">Thành công. Trả về thông báo xác nhận.</response>
    /// <response code="401">Chưa đăng nhập.</response>
    /// <response code="403">Không có quyền Admin.</response>
    /// <response code="404">Không tìm thấy gói nạp.</response>
    /// <response code="500">Lỗi server nội bộ.</response>
    [HttpPatch("{packageId:Guid}/inactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InactivatePaymentPackage([FromRoute] Guid packageId)
    {
        await packageService.InActivePaymentPackage(packageId);
        return Ok("Đã vô hiệu hóa gói thanh toán thành công");
    }
}