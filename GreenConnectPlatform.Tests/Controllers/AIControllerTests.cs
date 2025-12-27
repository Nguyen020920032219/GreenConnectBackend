using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.AI;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Services.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class AIControllerTests
{
    private readonly AIController _controller;
    private readonly Mock<IScrapRecognitionService> _mockAIService;
    private readonly Guid _testUserId;

    public AIControllerTests()
    {
        _mockAIService = new Mock<IScrapRecognitionService>();
        _controller = new AIController(_mockAIService.Object);

        _testUserId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task PST01_AnalyzeScrap_ReturnsResult_WhenImageIsValid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream();
        fileMock.Setup(f => f.OpenReadStream()).Returns(ms);
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.FileName).Returns("test.jpg"); // Setup tên file

        var expectedResult = new ScrapPostAiSuggestion
        {
            SuggestedTitle = "Test Title",
            SavedImageUrl = "http://test.com/img.jpg"
        };

        _mockAIService.Setup(s => s.AnalyzeImageAsync(fileMock.Object, _testUserId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.AnalyzeScrap(fileMock.Object);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<ScrapPostAiSuggestion>().Subject;
        data.SuggestedTitle.Should().Be("Test Title");
    }

    // [FIX LỖI 2] PST-02: Setup FileName để tránh NullReferenceException
    [Fact]
    public async Task PST02_AnalyzeScrap_ThrowsException_WhenServiceFails()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1024);
        fileMock.Setup(f => f.FileName).Returns("test.jpg"); // <--- QUAN TRỌNG: Phải có dòng này

        // Mock Service ném lỗi
        _mockAIService.Setup(s => s.AnalyzeImageAsync(fileMock.Object, _testUserId))
            .ThrowsAsync(new ApiExceptionModel(400, "AI_ERROR", "Cannot detect scrap"));

        // Act & Assert
        await _controller.Invoking(c => c.AnalyzeScrap(fileMock.Object))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400 && e.ErrorCode == "AI_ERROR");
    }

    // [FIX LỖI 1] PST-03: Test khớp với ErrorCode "FILE_MISSING" mới sửa ở Controller
    [Fact]
    public async Task PST03_AnalyzeScrap_ThrowsBadRequest_WhenFileIsEmpty()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0); // File rỗng

        // Act & Assert
        await _controller.Invoking(c => c.AnalyzeScrap(fileMock.Object))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400 && e.ErrorCode == "400");
    }
}