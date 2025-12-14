using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Notifications;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GreenConnectPlatform.Tests.Controllers
{
    public class NotificationControllerTests
    {
        private readonly Mock<INotificationService> _mockService;
        private readonly NotificationController _controller;
        private readonly Guid _testUserId;

        public NotificationControllerTests()
        {
            _mockService = new Mock<INotificationService>();
            _controller = new NotificationController(_mockService.Object);

            _testUserId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, _testUserId.ToString()),
                new Claim(ClaimTypes.Role, "Household")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        // ==========================================
        // 1. GET LIST (NOT-01)
        // ==========================================
        [Fact] // NOT-01
        public async Task NOT01_GetList_ReturnsOk_WithPaginatedData()
        {
            // Arrange
            var pagedResult = new PaginatedResult<NotificationModel>
            {
                Data = new List<NotificationModel> 
                { 
                    new NotificationModel { NotificationId = Guid.NewGuid(), Title = "Booking Accepted", IsRead = false } // [FIX] Property Name là NotificationId
                },
                Pagination = new PaginationModel(1, 1, 10)
            };

            _mockService.Setup(s => s.GetListAsync(_testUserId, 1, 10))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.GetList(1, 10); 

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<PaginatedResult<NotificationModel>>().Subject;
            data.Data.Should().HaveCount(1);
            data.Data[0].Title.Should().Be("Booking Accepted");
        }

        // ==========================================
        // 2. MARK AS READ (NOT-02)
        // ==========================================
        [Fact] // NOT-02: Success
        public async Task NOT02_ReadNotification_ReturnsOk_WhenExists()
        {
            // Arrange
            var notificationId = Guid.NewGuid();
            
            _mockService.Setup(s => s.ReadNotificationAsync(notificationId, _testUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ReadNotification(notificationId); 

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // NOT-02: Fail (Not Found)
        public async Task NOT02_ReadNotification_ThrowsNotFound_WhenMissing()
        {
            // Arrange
            var notificationId = Guid.NewGuid();

            _mockService.Setup(s => s.ReadNotificationAsync(notificationId, _testUserId))
                .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Notification not found"));

            // Act & Assert
            await _controller.Invoking(c => c.ReadNotification(notificationId))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 404);
        }

        // ==========================================
        // 3. MARK ALL AS READ (NOT-03)
        // ==========================================
        [Fact] // NOT-03
        public async Task NOT03_ReadAll_ReturnsOk_WhenSuccess()
        {
            // Arrange
            _mockService.Setup(s => s.ReadAllAsync(_testUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ReadAll(); 

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }
        
        // ==========================================
        // Extra: Register Device
        // ==========================================
        [Fact]
        public async Task RegisterDevice_ReturnsOk_WhenValid()
        {
            // Arrange
            var request = new RegisterDeviceRequest { FcmToken = "token_123" };

            _mockService.Setup(s => s.RegisterDeviceAsync(_testUserId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RegisterDevice(request);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}