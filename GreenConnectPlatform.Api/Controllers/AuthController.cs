using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Tags("1. Authentication (Xác thực & Phân quyền)")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    ///     (Mobile) Đăng nhập hoặc Đăng ký bằng Firebase Token.
    /// </summary>
    /// <remarks>
    ///     API chính dành cho Mobile App. <br />
    ///     **Quy trình xử lý:** <br />
    ///     1. Backend xác thực `FirebaseToken` gửi lên (lấy số điện thoại). <br />
    ///     2. Nếu SĐT chưa tồn tại -> **Tự động Đăng ký**: Tạo User mới (Role mặc định là `Household`), tạo Profile mặc định
    ///     -> Trả về `201 Created`. <br />
    ///     3. Nếu SĐT đã tồn tại -> **Đăng nhập**: Kiểm tra trạng thái tài khoản -> Trả về `200 OK`. <br />
    ///     <br />
    ///     **Các trường hợp bị chặn (Lỗi 403):** <br />
    ///     - User là Collector nhưng đang chờ duyệt (`PendingVerification`). <br />
    ///     - Tài khoản bị khóa (`Blocked`).
    /// </remarks>
    /// <param name="request">Chứa chuỗi `FirebaseToken` lấy từ Firebase SDK.</param>
    /// <response code="200">Đăng nhập thành công. Trả về Token và thông tin User.</response>
    /// <response code="201">Đăng ký thành công (User mới).</response>
    /// <response code="400">Token không hợp lệ hoặc thiếu thông tin SĐT.</response>
    /// <response code="401">Token hết hạn hoặc sai chữ ký.</response>
    /// <response code="403">Tài khoản chưa được duyệt hoặc bị khóa.</response>
    [HttpPost("login-or-register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> LoginOrRegister([FromBody] LoginOrRegisterRequest request)
    {
        var (authResponse, isNewUser) = await _authService.LoginOrRegisterAsync(request);

        if (isNewUser) return CreatedAtAction(nameof(ProfileController.GetMyProfile), "Profile", null, authResponse);

        return Ok(authResponse);
    }

    /// <summary>
    ///     (Web Admin) Đăng nhập trang quản trị.
    /// </summary>
    /// <remarks>
    ///     Dành riêng cho Web Portal. Sử dụng Email & Password truyền thống. <br />
    ///     Có hỗ trợ tài khoản Test (Backdoor) cho Developer (Mật khẩu: `@1`).
    /// </remarks>
    /// <param name="request">Email và Mật khẩu.</param>
    /// <response code="200">Đăng nhập thành công. Trả về Admin Token.</response>
    /// <response code="401">Sai mật khẩu.</response>
    /// <response code="403">User tồn tại nhưng không có quyền Admin.</response>
    /// <response code="404">Không tìm thấy tài khoản với Email này.</response>
    [HttpPost("admin-login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ExceptionModel), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var authResponse = await _authService.AdminLoginAsync(request);
        return Ok(authResponse);
    }
}