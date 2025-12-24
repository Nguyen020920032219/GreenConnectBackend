// using System.Security.Claims;
// using FluentAssertions;
// using GreenConnectPlatform.Api.Controllers;
// using GreenConnectPlatform.Business.Models.AI;
// using GreenConnectPlatform.Business.Models.Exceptions;
// using GreenConnectPlatform.Business.Services.AI;
// using GreenConnectPlatform.Business.Services.Storage;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
//
// namespace GreenConnectPlatform.Tests.Controllers;
//
// public class AIControllerTests
// {
//     private readonly AIController _controller;
//     private readonly Mock<IScrapRecognitionService> _mockAIService;
//     private readonly Mock<IStorageService> _mockStorageService;
//     private readonly Guid _testUserId;
//
//     public AIControllerTests()
//     {
//         _mockAIService = new Mock<IScrapRecognitionService>();
//         _mockStorageService = new Mock<IStorageService>();
//         _controller = new AIController(_mockAIService.Object, _mockStorageService.Object);
//
//         _testUserId = Guid.NewGuid();
//         var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
//         {
//             new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString())
//         }, "mock"));
//
//         _controller.ControllerContext = new ControllerContext
//         {
//             HttpContext = new DefaultHttpContext { User = user }
//         };
//     }
//
//     // PST-01: Analyze scrap image successfully
//     [Fact]
//     public async Task PST01_RecognizeScrap_ReturnsResult_WhenImageIsValid()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         var ms = new MemoryStream();
//         fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
//         fileMock.Setup(_ => _.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
//             .Returns(Task.CompletedTask);
//
//         var aiResponse = new ScrapRecognitionResponse
//         {
//             // Populate expected AI response fields
//         };
//
//         // Mock AI Service
//         _mockAIService.Setup(s => s.RecognizeScrapImageAsync(fileMock.Object))
//             .ReturnsAsync(aiResponse);
//
//         // Mock Storage Service (Direct Upload)
//         _mockStorageService.Setup(s => s.UploadScrapImageDirectAsync(_testUserId, fileMock.Object))
//             .ReturnsAsync("path/to/image.jpg");
//
//         // Mock Storage Service (Get URL)
//         _mockStorageService.Setup(s => s.GetFileReadUrlAsync(_testUserId, "", "path/to/image.jpg"))
//             .ReturnsAsync("https://firebase.url/image.jpg");
//
//         // Act
//         var result = await _controller.RecognizeScrap(fileMock.Object);
//
//         // Assert
//         var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
//         var data = okResult.Value.Should().BeOfType<ScrapRecognitionResponse>().Subject;
//         data.SavedImageUrl.Should().Be("https://firebase.url/image.jpg");
//     }
//
//     // PST-02: Analyze scrap image fail (AI Service Error)
//     [Fact]
//     public async Task PST02_RecognizeScrap_ThrowsException_WhenAnalysisFails()
//     {
//         // Arrange
//         var fileMock = new Mock<IFormFile>();
//         var ms = new MemoryStream();
//         fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
//
//         // Mock AI Service throwing error
//         _mockAIService.Setup(s => s.RecognizeScrapImageAsync(fileMock.Object))
//             .ThrowsAsync(new ApiExceptionModel(400, "AI_ERROR", "Cannot detect scrap"));
//
//         // Act & Assert
//         await _controller.Invoking(c => c.RecognizeScrap(fileMock.Object))
//             .Should().ThrowAsync<ApiExceptionModel>();
//     }
// }