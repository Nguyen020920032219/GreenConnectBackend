using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.PaymentTransactions;
using GreenConnectPlatform.Business.Services.PaymentTransactions;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class PaymentTransactionControllerTests
{
    private readonly PaymentTransactionController _controller;
    private readonly Mock<IPaymentTransactionService> _mockService;
    private readonly Guid _testUserId;

    public PaymentTransactionControllerTests()
    {
        _mockService = new Mock<IPaymentTransactionService>();
        _controller = new PaymentTransactionController(_mockService.Object);
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
    // PAY-13: Payment History
    // ==========================================
    [Fact] // PAY-13 Xem lịch sử thanh toán của tôi
    public async Task PAY13_GetMyHistory_ReturnsOk_WithList()
    {
        // Arrange
        var page = 1;
        var size = 10;
        var sortByDate = true;

        // Giả lập dữ liệu trả về
        var pagedResult = new PaginatedResult<PaymentTransactionModel>
        {
            Data = new List<PaymentTransactionModel>
            {
                new()
                {
                    PaymentId = Guid.NewGuid(),
                    Amount = 100000,
                    Status = PaymentStatus.Success,
                    PaymentGateway = "VNPay"
                }
            },
            Pagination = new PaginationModel(1, 1, 10)
        };

        // Setup Mock: Gọi hàm GetPaymentTransactionsByUserAsync
        _mockService.Setup(s => s.GetPaymentTransactionsByUserAsync(
                page, size, _testUserId, sortByDate, null))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetPaymentTransactionsByUser(page, size, sortByDate);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<PaginatedResult<PaymentTransactionModel>>().Subject;

        data.Data.Should().HaveCount(1);
        data.Data[0].Amount.Should().Be(100000);

        // Verify service được gọi đúng tham số
        _mockService.Verify(s => s.GetPaymentTransactionsByUserAsync(
            page, size, _testUserId, sortByDate, null), Times.Once);
    }

    [Fact] // PAY-14 Xem lịch sử thanh toán hệ thống (Admin)
    public async Task PAY14_GetPaymentTransactions_ReturnsOk_WithSystemHistory()
    {
        // Arrange
        // Đổi User Context sang Admin
        var adminUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));
        _controller.ControllerContext.HttpContext.User = adminUser;

        // Tham số đầu vào cho Admin
        var start = DateTime.Now.AddDays(-7);
        var end = DateTime.Now;
        var pageIndex = 1;
        var pageSize = 10;
        var sortByCreatedAt = true;
        PaymentStatus? status = null;

        var pagedResult = new PaginatedResult<PaymentTransactionModel>
        {
            Data = new List<PaymentTransactionModel>
            {
                new() { Amount = 500000, Status = PaymentStatus.Success },
                new() { Amount = 20000, Status = PaymentStatus.Failed }
            },
            Pagination = new PaginationModel(2, 1, 10)
        };

        // Setup Mock: Gọi hàm GetPaymentTransactionsAsync
        // Lưu ý: Controller có logic xử lý DateTime (ToUniversalTime), nên ở mock ta dùng It.IsAny<DateTime>
        // để tránh sai lệch milliseconds hoặc múi giờ khi verify.
        _mockService.Setup(s => s.GetPaymentTransactionsAsync(
                pageIndex, pageSize, sortByCreatedAt, status, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetPaymentTransactions(start, end, pageIndex, pageSize, sortByCreatedAt, status);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<PaginatedResult<PaymentTransactionModel>>().Subject;

        data.Data.Should().HaveCount(2);

        // Verify
        _mockService.Verify(s => s.GetPaymentTransactionsAsync(
            pageIndex, pageSize, sortByCreatedAt, status, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
    }
}