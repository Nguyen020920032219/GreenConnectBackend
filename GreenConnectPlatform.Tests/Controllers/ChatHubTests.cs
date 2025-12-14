using System.Reflection;
using FluentAssertions;
using GreenConnectPlatform.Business.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class ChatHubTests
{
    private readonly Mock<IGroupManager> _mockGroups;
    private readonly Mock<HubCallerContext> _mockContext;
    private readonly Mock<IHubCallerClients> _mockClients;
    private readonly ChatHub _chatHub;

    public ChatHubTests()
    {
        _mockGroups = new Mock<IGroupManager>();
        _mockContext = new Mock<HubCallerContext>();
        _mockClients = new Mock<IHubCallerClients>();

        _chatHub = new ChatHub
        {
            Groups = _mockGroups.Object,
            Context = _mockContext.Object,
            Clients = _mockClients.Object
        };
    }

    // ==========================================
    // Group 1 : SignalR Connect
    // ==========================================

    [Fact] // CHT-01 Kết nối ChatHub thành công
    public async Task CHT01_JoinUserTopic_AddsConnectionToGroup_WhenCalled()
    {
        // Arrange
        var connectionId = "conn_123";
        var userId = Guid.NewGuid().ToString();

        _mockContext.Setup(c => c.ConnectionId).Returns(connectionId);
        _mockGroups.Setup(g => g.AddToGroupAsync(connectionId, It.IsAny<string>(), default))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        await _chatHub.JoinUserTopic(userId);

        // Assert
        _mockGroups.Verify(g => g.AddToGroupAsync(
            connectionId,
            $"User_{userId.ToLower()}",
            default), Times.Once);
    }

    [Fact] // CHT-02 Kết nối ChatHub thất bại - Không cho phép
    public void CHT02_ChatHub_ShouldBeDecoratedWithAuthorizeAttribute()
    {
        // Arrange
        var type = typeof(ChatHub);

        // Act
        // Lấy attribute [Authorize] của class ChatHub
        var authorizeAttribute = type.GetCustomAttribute<AuthorizeAttribute>();

        // Assert
        authorizeAttribute.Should().NotBeNull("ChatHub must be secured with [Authorize] attribute");
    }
}