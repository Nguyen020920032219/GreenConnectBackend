using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Reports;
using GreenConnectPlatform.Business.Services.Reports;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ReportControllerTests
{
    private readonly Guid _adminId;
    private readonly ReportController _controller;
    private readonly Mock<IReportService> _mockService;

    public ReportControllerTests()
    {
        _mockService = new Mock<IReportService>();
        _controller = new ReportController(_mockService.Object);

        _adminId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _adminId.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // --- ADM-11, ADM-12: Get Admin Report (Dashboard) ---
    [Fact]
    public async Task ADM11_GetReport_ReturnsOk_WithStats()
    {
        // Arrange
        var start = DateTime.Now.AddDays(-30);
        var end = DateTime.Now;
        var reportData = new ReportModel(); // Giả lập dữ liệu

        // Lưu ý: Controller có logic xử lý DateTimeKind, nhưng ở đây ta test Service call
        _mockService.Setup(s => s.GetReport(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(reportData);

        // Act
        var result = await _controller.GetReport(start, end);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeOfType<ReportModel>();
    }
}