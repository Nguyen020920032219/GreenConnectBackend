using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Files;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Profile;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace GreenConnectPlatform.Tests.Controllers
{
    public class ProfileControllerTests
    {
        private readonly Mock<IProfileService> _mockProfileService;
        private readonly ProfileController _controller;
        private readonly Guid _testUserId;

        public ProfileControllerTests()
        {
            _mockProfileService = new Mock<IProfileService>();
            _controller = new ProfileController(_mockProfileService.Object);

            // Giả lập User đang đăng nhập (Role Household)
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
        // GROUP 1: Get Profile (PF-01, PF-02)
        // ==========================================

        [Fact] // PF-01: Lấy thông tin hồ sơ thành công
        public async Task PF01_GetMyProfile_ReturnsOk_WhenUserExists()
        {
            // Arrange
            var expectedProfile = new ProfileModel 
            { 
                UserId = _testUserId, 
                FullName = "Nguyen Van A",
                PhoneNumber = "0901234567",
                Rank = "Gold"
            };

            _mockProfileService.Setup(s => s.GetMyProfileAsync(_testUserId))
                .ReturnsAsync(expectedProfile);

            // Act
            var result = await _controller.GetMyProfile();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var data = okResult.Value.Should().BeOfType<ProfileModel>().Subject;
            data.UserId.Should().Be(_testUserId);
            data.FullName.Should().Be("Nguyen Van A");
        }

        [Fact] // PF-02: Lấy hồ sơ thất bại (User không tồn tại/bị xóa)
        public async Task PF02_GetMyProfile_ThrowsNotFound_WhenUserMissing()
        {
            // Arrange
            _mockProfileService.Setup(s => s.GetMyProfileAsync(_testUserId))
                .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "Profile not found"));

            // Act & Assert
            await _controller.Invoking(c => c.GetMyProfile())
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 404);
        }

        // ==========================================
        // GROUP 2: Update Profile (PF-03, PF-04, PF-05)
        // ==========================================

        [Fact] // PF-03: Cập nhật thông tin thành công
        public async Task PF03_UpdateMyProfile_ReturnsOk_WhenValidData()
        {
            // Arrange
            var request = new UpdateProfileRequest { FullName = "New Name", Gender = Gender.Male };
            var updatedProfile = new ProfileModel { UserId = _testUserId, FullName = "New Name" };

            _mockProfileService.Setup(s => s.UpdateMyProfileAsync(_testUserId, request))
                .ReturnsAsync(updatedProfile);

            // Act
            var result = await _controller.UpdateMyProfile(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ProfileModel)okResult.Value).FullName.Should().Be("New Name");
        }

        [Fact] // PF-04: Cập nhật thất bại - Tên rỗng (Validation)
        public async Task PF04_UpdateMyProfile_ThrowsBadRequest_WhenNameEmpty()
        {
            // Arrange
            var request = new UpdateProfileRequest { FullName = "" }; // Invalid
            
            _mockProfileService.Setup(s => s.UpdateMyProfileAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Full name is required"));

            // Act & Assert
            await _controller.Invoking(c => c.UpdateMyProfile(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }

        [Fact] // PF-05: Cập nhật một phần (Partial Update - Chỉ đổi địa chỉ)
        public async Task PF05_UpdateMyProfile_ReturnsOk_WhenPartialUpdate()
        {
            // Arrange
            var request = new UpdateProfileRequest { Address = "123 New Street" }; // Các trường khác null
            var updatedProfile = new ProfileModel { UserId = _testUserId, Address = "123 New Street" };

            _mockProfileService.Setup(s => s.UpdateMyProfileAsync(_testUserId, request))
                .ReturnsAsync(updatedProfile);

            // Act
            var result = await _controller.UpdateMyProfile(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ProfileModel)okResult.Value).Address.Should().Be("123 New Street");
        }

        // ==========================================
        // GROUP 3: Update Avatar (PF-06, PF-07)
        // ==========================================

        [Fact] // PF-06: Cập nhật Avatar thành công
        public async Task PF06_UpdateAvatar_ReturnsOk_WhenSuccess()
        {
            // Arrange
            var request = new UpdateFileRequestModel { FileName = "avatars/new-pic.jpg" };
            
            _mockProfileService.Setup(s => s.UpdateAvatarAsync(_testUserId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAvatar(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // PF-07: Cập nhật Avatar thất bại - Thiếu tên file
        public async Task PF07_UpdateAvatar_ThrowsException_WhenFileNameMissing()
        {
            // Arrange
            var request = new UpdateFileRequestModel { FileName = "" };

            _mockProfileService.Setup(s => s.UpdateAvatarAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "INVALID_FILE", "File name is required"));

            // Act & Assert
            await _controller.Invoking(c => c.UpdateAvatar(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }

        // ==========================================
        // GROUP 4: Submit eKYC (KYC-01, KYC-02, KYC-03)
        // ==========================================

        [Fact] // KYC-01: Gửi yêu cầu xác minh thành công
        public async Task KYC01_SubmitVerification_ReturnsOk_WhenSuccess()
        {
            // Arrange
            // Mock IFormFile vì request dùng FromForm
            var mockFile = new Mock<IFormFile>();
            var request = new SubmitEkycRequest 
            { 
                BuyerType = BuyerType.Individual, 
                FrontImage = mockFile.Object 
            };

            _mockProfileService.Setup(s => s.SubmitVerificationAsync(_testUserId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.SubmitVerification(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact] // KYC-02: Gửi yêu cầu thất bại - Đã xác minh rồi
        public async Task KYC02_SubmitVerification_ThrowsException_WhenAlreadyVerified()
        {
            // Arrange
            var request = new SubmitEkycRequest();

            _mockProfileService.Setup(s => s.SubmitVerificationAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "ALREADY_VERIFIED", "User is already verified"));

            // Act & Assert
            await _controller.Invoking(c => c.SubmitVerification(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }

        [Fact] // KYC-03: Gửi yêu cầu thất bại - Dưới 18 tuổi (Check từ Service)
        public async Task KYC03_SubmitVerification_ThrowsException_WhenUnder18()
        {
            // Arrange
            var request = new SubmitEkycRequest();

            _mockProfileService.Setup(s => s.SubmitVerificationAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "AGE_INVALID", "Must be 18+"));

            // Act & Assert
            await _controller.Invoking(c => c.SubmitVerification(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.ErrorCode == "AGE_INVALID");
        }
    }
}