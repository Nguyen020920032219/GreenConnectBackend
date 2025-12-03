using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/profile")]
[Authorize]
[Tags("02. Profile & Verification (Hồ sơ & Xác minh)")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    ///     (All) Xem hồ sơ cá nhân của tôi.
    /// </summary>
    /// <remarks>
    ///     Lấy đầy đủ thông tin chi tiết: Họ tên, SĐT, Địa chỉ, Giới tính, Ngày sinh, Điểm thưởng, Hạng thành viên (Rank),
    ///     Avatar...
    /// </remarks>
    /// <response code="200">Thành công. Trả về `ProfileModel`.</response>
    /// <response code="404">Không tìm thấy hồ sơ (Lỗi dữ liệu).</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetMyProfileAsync(userId);
        return Ok(profile);
    }

    /// <summary>
    ///     (All) Cập nhật thông tin cá nhân.
    /// </summary>
    /// <remarks>
    ///     Cho phép sửa các trường: Họ tên, Địa chỉ, Giới tính, Ngày sinh. <br />
    ///     Các trường gửi lên là `null` sẽ được giữ nguyên giá trị cũ (Partial Update).
    /// </remarks>
    /// <param name="request">Thông tin cần sửa.</param>
    /// <response code="200">Cập nhật thành công.</response>
    [HttpPut("me")]
    [ProducesResponseType(typeof(ProfileModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = GetCurrentUserId();
        var updatedProfile = await _profileService.UpdateMyProfileAsync(userId, request);
        return Ok(updatedProfile);
    }

    /// <summary>
    ///     (All) Cập nhật ảnh đại diện (Avatar).
    /// </summary>
    /// <remarks>
    ///     **Lưu ý:** Client cần upload ảnh lên Cloud trước (qua API `Files`), sau đó gửi đường dẫn file (FileName) vào đây.
    ///     <br />
    ///     Hệ thống sẽ lưu đường dẫn mới và tự động xóa ảnh cũ trên Cloud để tiết kiệm dung lượng.
    /// </remarks>
    /// <param name="request">Đường dẫn file ảnh vừa upload.</param>
    /// <response code="200">Cập nhật thành công.</response>
    [HttpPost("avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateFileRequestModel request)
    {
        var userId = GetCurrentUserId();
        await _profileService.UpdateAvatarAsync(userId, request);
        return Ok(new { Message = "Cập nhật ảnh đại diện thành công." });
    }

    /// <summary>
    ///     (Household) Nộp hồ sơ nâng cấp lên Người thu gom (Collector).
    /// </summary>
    /// <remarks>
    ///     Dùng cho Household muốn đăng ký trở thành Collector (Cá nhân/Vựa). <br />
    ///     Gửi ảnh CCCD (mặt trước/sau) hoặc Giấy phép kinh doanh. <br />
    ///     **Hệ quả:** <br />
    ///     - Tài khoản chuyển sang trạng thái `PendingVerification`. <br />
    ///     - Role chuyển sang `IndividualCollector` hoặc `BusinessCollector`. <br />
    ///     - Chờ Admin duyệt.
    /// </remarks>
    /// <param name="request">Loại tài khoản muốn nâng và link ảnh giấy tờ.</param>
    /// <response code="200">Nộp hồ sơ thành công.</response>
    [HttpPost("verification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SubmitVerification([FromBody] SubmitVerificationRequest request)
    {
        var userId = GetCurrentUserId();
        await _profileService.SubmitVerificationAsync(userId, request);
        return Ok(new { Message = "Gửi thông tin xác minh thành công. Vui lòng chờ Admin duyệt." });
    }

    private Guid GetCurrentUserId()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdStr, out var userId)) return userId;
        throw new UnauthorizedAccessException("Invalid User Token");
    }
}