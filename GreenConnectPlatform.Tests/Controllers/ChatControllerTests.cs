using System.Security.Claims;
using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ChatControllerTests
{
    private readonly ChatController _controller;
    private readonly Mock<IChatService> _mockService;
    private readonly Guid _testUserId;

    public ChatControllerTests()
    {
        _mockService = new Mock<IChatService>();
        _controller = new ChatController(_mockService.Object);

        _testUserId = Guid.NewGuid();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
            new Claim(ClaimTypes.Role, "Household")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    // ==========================================
    // CHT-03: Send Text Message successfully
    // ==========================================

    [Fact] // CHT-03
    public async Task CHT03_SendMessage_ReturnsOk_WhenTextValid()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var request = new SendMessageModel
        {
            TransactionId = transactionId,
            Content = "Hello world"
        };

        var resultMessage = new MessageModel { MessageId = Guid.NewGuid(), Content = "Hello world" };

        _mockService.Setup(s => s.SendMessageAsync(_testUserId, request))
            .ReturnsAsync(resultMessage);

        // Act
        var result = await _controller.SendMessage(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((MessageModel)okResult.Value).Content.Should().Be("Hello world");
    }

    // ==========================================
    // CHT-08: Send Image Message successfully
    // ==========================================

    [Fact] // CHT-08
    public async Task CHT08_SendMessage_ReturnsOk_WhenImageValid()
    {
        // Arrange
        var request = new SendMessageModel
        {
            TransactionId = Guid.NewGuid(),
            Content = "https://storage.com/img.jpg"
        };

        var resultMessage = new MessageModel { MessageId = Guid.NewGuid(), Content = request.Content };

        _mockService.Setup(s => s.SendMessageAsync(_testUserId, request))
            .ReturnsAsync(resultMessage);

        // Act
        var result = await _controller.SendMessage(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var msg = okResult.Value.Should().BeOfType<MessageModel>().Subject;
        // Note: MessageModel có thể sử dụng MessageType thay vì Type. Dùng MessageType
        msg.Content.Should().Be("https://storage.com/img.jpg");
    }

    // ==========================================
    // CHT-04: Send Fail - Empty Content
    // ==========================================
    [Fact] // CHT-04
    public async Task CHT04_SendMessage_ThrowsBadRequest_WhenContentEmpty()
    {
        // Arrange
        var request = new SendMessageModel { TransactionId = Guid.NewGuid(), Content = "" };

        _mockService.Setup(s => s.SendMessageAsync(_testUserId, request))
            .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Content is required"));

        // Act & Assert
        await _controller.Invoking(c => c.SendMessage(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    // ==========================================
    // CHT-06: Send Fail - Too Long
    // ==========================================
    [Fact] // CHT-06
    public async Task CHT06_SendMessage_ThrowsBadRequest_WhenContentTooLong()
    {
        // Arrange
        var request = new SendMessageModel { TransactionId = Guid.NewGuid(), Content = new string('a', 5001) };

        _mockService.Setup(s => s.SendMessageAsync(_testUserId, request))
            .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Message too long"));

        // Act & Assert
        await _controller.Invoking(c => c.SendMessage(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    // ==========================================
    // CHT-07: Get Inbox (List Conversations)
    // ==========================================
    [Fact] // CHT-07
    public async Task CHT07_GetConversations_ReturnsOk_WithList()
    {
        // Arrange
        var pagedResult = new PaginatedResult<ChatRoomModel>
        {
            Data = new List<ChatRoomModel>
            {
                new() { ChatRoomId = Guid.NewGuid(), LastMessage = "Hi" }
            },
            Pagination = new PaginationModel(1, 1, 10)
        };

        _mockService.Setup(s => s.GetMyChatRoomAsync(_testUserId, null, 1, 10))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetMyRooms(null);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = okResult.Value.Should().BeOfType<PaginatedResult<ChatRoomModel>>().Subject;
        data.Data.Should().HaveCount(1);
    }

    // ==========================================
    // Extra: Get Messages in Room
    // ==========================================
    [Fact]
    public async Task GetMessages_ReturnsOk_WithHistory()
    {
        // Arrange
        var chatRoomId = Guid.NewGuid();
        var pagedResult = new PaginatedResult<MessageModel>
        {
            Data = new List<MessageModel> { new() { Content = "Old msg" } },
            Pagination = new PaginationModel(1, 1, 10)
        };

        _mockService.Setup(s => s.GetChatHistoryAsync(1, 20, chatRoomId))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _controller.GetMessages(chatRoomId, 1, 20);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        ((PaginatedResult<MessageModel>)okResult.Value).Data.Should().HaveCount(1);
    }
}