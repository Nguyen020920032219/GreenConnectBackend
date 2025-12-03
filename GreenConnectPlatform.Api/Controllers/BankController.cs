using GreenConnectPlatform.Business.Models.Banks;
using GreenConnectPlatform.Business.Services.Banks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenConnectPlatform.Api.Controllers;

[ApiController]
[Route("api/v1/banks")]
[Tags("19. Bank (Danh Sách Ngân Hàng)")]
public class BankController : ControllerBase
{
    private readonly IBankService _bankService;

    public BankController(IBankService bankService)
    {
        _bankService = bankService;
    }

    /// <summary>
    ///     (Public) Lấy danh sách ngân hàng hỗ trợ VietQR.
    /// </summary>
    /// <remarks>
    ///     Dùng để hiển thị Dropdown chọn ngân hàng trong màn hình Cập nhật Profile. <br />
    ///     **Lưu ý:** Khi User chọn ngân hàng, Client cần lấy trường `Bin` (Mã số ngân hàng) để gửi lên API Update Profile
    ///     (trường `BankCode`).
    /// </remarks>
    /// <response code="200">Danh sách ngân hàng (Tên, Mã, Logo...).</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<BankModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var banks = await _bankService.GetSupportedBanksAsync();
        return Ok(banks);
    }
}