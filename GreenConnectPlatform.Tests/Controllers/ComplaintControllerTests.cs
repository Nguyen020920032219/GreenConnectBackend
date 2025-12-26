using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Complaints;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Complaints;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ComplaintControllerTests
{
    private readonly Guid _adminId;
    private readonly ComplaintController _controller;
    private readonly Mock<IComplaintService> _mockService;

    public ComplaintControllerTests()
    {
        _mockService = new Mock<IComplaintService>();
        _controller = new ComplaintController(_mockService.Object);

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

    // --- ADM-09: View Complaint List ---
    [Fact]
    public async Task ADM09_GetComplaints_ReturnsOk_WithData()
    {
        // Arrange
        var pagedResult = new PaginatedResult<ComplaintModel>
        {
            Data = new List<ComplaintModel>
                { new() { ComplaintId = Guid.NewGuid(), Status = ComplaintStatus.Submitted } },
            Pagination = new PaginationModel(1, 1, 10)
        };

        // Setup mock khớp tham số: page, size, sortDate, sortStatus, userId, userRole
        _mockService.Setup(s =>
                s.GetComplaints(1, 10, true, ComplaintStatus.Submitted, It.IsAny<Guid?>(), It.IsAny<string>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetComplaints(1, 10, true, ComplaintStatus.Submitted);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<PaginatedResult<ComplaintModel>>().Subject;
        data.Data.Should().HaveCount(1);
    }

    // --- ADM-10: Process Complaint (Accept/Reject) ---
    [Fact]
    public async Task ADM10_ProcessComplaint_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var complaintId = Guid.NewGuid();
        var isAccept = true;

        _mockService.Setup(s => s.ProcessComplaintAsync(complaintId, isAccept))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ProcessComplaint(complaintId, isAccept);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be("Xử lý khiếu nại thành công");
    }
}