using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.ScheduleProposals;
using GreenConnectPlatform.Business.Services.ScheduleProposals;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ScheduleProposalControllerTests
{
    private readonly ScheduleProposalController _controller;
    private readonly Mock<IScheduleProposalService> _mockService;
    private readonly Guid _testUserId;

    public ScheduleProposalControllerTests()
    {
        _mockService = new Mock<IScheduleProposalService>();
        _controller = new ScheduleProposalController(_mockService.Object);

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
    // 1. CREATE & GET (BOK-13, BOK-14)
    // ==========================================

    [Fact] // BOK-13: Create reschedule proposal
    public async Task BOK13_Create_ReturnsCreated_WhenValid()
    {
        // Arrange
        var offerId = Guid.NewGuid();
        var request = new ScheduleProposalCreateModel
        {
            ProposedTime = DateTime.Now.AddDays(1),
            ResponseMessage = "Change time"
        };

        var createdProposal = new ScheduleProposalModel
            { ScheduleProposalId = Guid.NewGuid(), Status = ProposalStatus.Pending };

        _mockService.Setup(s => s.CreateAsync(_testUserId, offerId, request))
            .ReturnsAsync(createdProposal);

        // Act
        var result = await _controller.Create(offerId, request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        ((ScheduleProposalModel)createdResult.Value).Status.Should().Be(ProposalStatus.Pending);
    }

    [Fact] // BOK-14: Collector views proposal history
    public async Task BOK14_GetMyProposals_ReturnsOk()
    {
        // Arrange
        var pagedResult = new PaginatedResult<ScheduleProposalModel>
        {
            Data = new List<ScheduleProposalModel>
                { new() { ScheduleProposalId = Guid.NewGuid() } },
            Pagination = new PaginationModel(1, 1, 10)
        };

        _mockService.Setup(s => s.GetByCollectorAsync(1, 10, null, true, _testUserId))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetMyProposals(null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((PaginatedResult<ScheduleProposalModel>)okResult.Value).Data.Should().HaveCount(1);
    }

    // ==========================================
    // 2. UPDATE & CANCEL (BOK-15, BOK-16)
    // ==========================================

    [Fact] // BOK-15: Update proposal details
    public async Task BOK15_Update_ReturnsOk_WhenValid()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var newTime = DateTime.Now.AddDays(2);
        var newMessage = "Updated note";
        var updatedProposal = new ScheduleProposalModel
            { ScheduleProposalId = proposalId, ResponseMessage = newMessage };

        _mockService.Setup(s => s.UpdateAsync(_testUserId, proposalId, newTime, newMessage))
            .ReturnsAsync(updatedProposal);

        // Act
        var result = await _controller.Update(proposalId, newTime, newMessage);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((ScheduleProposalModel)okResult.Value).ResponseMessage.Should().Be(newMessage);
    }

    [Fact] // BOK-16: Cancel proposal
    public async Task BOK16_ToggleCancel_ReturnsOk_WhenSuccess()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        _mockService.Setup(s => s.ToggleCancelAsync(_testUserId, proposalId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ToggleCancel(proposalId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    // ==========================================
    // 3. PROCESS PROPOSAL (BOK-17, BOK-18)
    // ==========================================

    [Fact] // BOK-17: Household accepts reschedule
    public async Task BOK17_ProcessProposal_Accept_ReturnsOk()
    {
        // Arrange - Switch to Household
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Role, "Household"),
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
            }, "mock"));
        _controller.ControllerContext.HttpContext.User = user;

        var proposalId = Guid.NewGuid();
        _mockService.Setup(s => s.ProcessProposalAsync(_testUserId, proposalId, true, null))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ProcessProposal(proposalId, true, null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }

    [Fact] // BOK-18: Household rejects reschedule
    public async Task BOK18_ProcessProposal_Reject_ReturnsOk()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Role, "Household"),
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
            }, "mock"));
        _controller.ControllerContext.HttpContext.User = user;

        var proposalId = Guid.NewGuid();
        var reason = "Busy";
        _mockService.Setup(s => s.ProcessProposalAsync(_testUserId, proposalId, false, reason))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ProcessProposal(proposalId, false, reason);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }
}