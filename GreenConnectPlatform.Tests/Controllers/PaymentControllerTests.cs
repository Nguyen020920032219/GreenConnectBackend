using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Payment;
using GreenConnectPlatform.Business.Services.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class PaymentControllerTests
{
    private readonly PaymentController _controller;
    private readonly Mock<IPaymentService> _mockPaymentService;
    private readonly Guid _testUserId;

    public PaymentControllerTests()
    {
        _mockPaymentService = new Mock<IPaymentService>();

        // [FIX] Constructor chỉ nhận 1 tham số IPaymentService (theo file PaymentController.cs bạn gửi)
        _controller = new PaymentController(_mockPaymentService.Object);

        _testUserId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
            new Claim(ClaimTypes.Role, "IndividualCollector")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // ==========================================
    // PAY-06: Create Payment URL
    // ==========================================
    [Fact]
    public async Task PAY06_CreateUrl_ReturnsOk_WithUrl()
    {
        // Arrange
        var request = new CreatePaymentRequest { PackageId = Guid.NewGuid() };
        var response = new PaymentLinkResponse { PaymentUrl = "https://sandbox.vnpayment.vn/..." };

        _mockPaymentService.Setup(s => s.CreatePaymentLinkAsync(_testUserId, request, It.IsAny<HttpContext>()))
            .ReturnsAsync(response);

        // Act
        // [FIX] Tên hàm là CreateUrl
        var result = await _controller.CreateUrl(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((PaymentLinkResponse)okResult.Value).PaymentUrl.Should().Contain("https://sandbox");
    }

    [Fact] // PAY-07: Create Fail
    public async Task PAY07_CreateUrl_ThrowsException_WhenPackageInvalid()
    {
        // Arrange
        var request = new CreatePaymentRequest { PackageId = Guid.NewGuid() };

        _mockPaymentService.Setup(s => s.CreatePaymentLinkAsync(_testUserId, request, It.IsAny<HttpContext>()))
            .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Package not found"));

        // Act & Assert
        // [FIX] Tên hàm là CreateUrl, check ThrowAsync<ApiExceptionModel>
        await _controller.Invoking(c => c.CreateUrl(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 404);
    }

    // ==========================================
    // PAY-08, PAY-09, PAY-10: Payment Return (Redirect)
    // ==========================================
    [Fact] // PAY-09: Success Redirect (Code 00)
    public void PAY09_PaymentReturn_RedirectsToSuccess_WhenCode00()
    {
        // Arrange
        var query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { "vnp_ResponseCode", "00" }
        });
        _controller.ControllerContext.HttpContext.Request.Query = query;

        // Act
        // [FIX] Tên hàm là PaymentReturn
        var result = _controller.PaymentReturn();

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Contain("status=success");
    }

    [Fact] // PAY-08 & PAY-10: Failed Redirect (Code != 00)
    public void PAY10_PaymentReturn_RedirectsToFailed_WhenCodeNot00()
    {
        // Arrange
        var query = new QueryCollection(new Dictionary<string, StringValues>
        {
            { "vnp_ResponseCode", "24" } // Cancel
        });
        _controller.ControllerContext.HttpContext.Request.Query = query;

        // Act
        var result = _controller.PaymentReturn();

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectResult>().Subject;
        redirectResult.Url.Should().Contain("status=failed");
    }

    // ==========================================
    // PAY-11, PAY-12: Payment IPN (Background Call)
    // ==========================================
    [Fact] // PAY-11: IPN Success
    public async Task PAY11_PaymentIpn_ReturnsRspCode00_WhenSuccess()
    {
        // Arrange
        _controller.ControllerContext.HttpContext.Request.Query = new QueryCollection();

        _mockPaymentService.Setup(s => s.ProcessPaymentCallbackAsync(It.IsAny<IQueryCollection>()))
            .Returns(Task.CompletedTask);

        // Act
        // [FIX] Tên hàm là PaymentIpn
        var result = await _controller.PaymentIpn();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        // Dùng dynamic hoặc reflection để check anonymous object
        var val = okResult.Value;
        val.GetType().GetProperty("RspCode").GetValue(val, null).Should().Be("00");
    }

    [Fact] // PAY-12: IPN Fail (Exception from Service)
    public async Task PAY12_PaymentIpn_ReturnsRspCode99_WhenException()
    {
        // Arrange
        _controller.ControllerContext.HttpContext.Request.Query = new QueryCollection();

        _mockPaymentService.Setup(s => s.ProcessPaymentCallbackAsync(It.IsAny<IQueryCollection>()))
            .ThrowsAsync(new ApiExceptionModel(400, "ERROR", "Invalid signature"));

        // Act
        var result = await _controller.PaymentIpn();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var val = okResult.Value;
        val.GetType().GetProperty("RspCode").GetValue(val, null).Should().Be("99");
    }
}