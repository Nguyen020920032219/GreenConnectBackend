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
using System.Security.Claims;
using GreenConnectPlatform.Business.Models.ScrapPosts;
using GreenConnectPlatform.Business.Models.VerificationInfos;

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
        

        
        // ==========================================
        // GROUP 3: Update Avatar (PF-05, PF-06, PF-07)
        // ==========================================

        [Fact] // PF-05: Cập nhật Avatar thành công
        public async Task PF05_UpdateAvatar_ReturnsOk_WhenSuccess()
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

        [Fact] // PF-06: Cập nhật Avatar thất bại - Thiếu tên file
        public async Task PF06_UpdateAvatar_ThrowsException_WhenFileNameMissing()
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
        
        [Fact] // PF-07: Cập nhật Avatar thất bại - Sai định dạng file
        public async Task PF07_UpdateAvatar_ThrowsException_WhenInvalidFormat()
        {
            // Arrange
            var request = new UpdateFileRequestModel { FileName = "text.txt" };

            _mockProfileService.Setup(s => s.UpdateAvatarAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "INVALID_FILE", "Invalid file format"));

            // Act & Assert
            await _controller.Invoking(c => c.UpdateAvatar(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400);
        }
        
        // ==========================================
        // GROUP 4: Update Address (PF-08, PF-09)
        // ==========================================
        
        [Fact] // PF-08: Cập nhật một phần (Partial Update - Chỉ đổi địa chỉ)
        public async Task PF08_UpdateMyProfile_ReturnsOk_WhenPartialUpdate()
        {
            var location = new LocationModel
            {
                Latitude = 10.762,
                Longitude = 106.660
            };
            // Arrange
            var request = new UpdateProfileRequest { Address = "123 Street", Location  = location}; // Các trường khác null
            var updatedProfile = new ProfileModel { UserId = _testUserId, Address = "123 Street" };

            _mockProfileService.Setup(s => s.UpdateMyProfileAsync(_testUserId, request))
                .ReturnsAsync(updatedProfile);

            // Act
            var result = await _controller.UpdateMyProfile(request);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            ((ProfileModel)okResult.Value).Address.Should().Be("123 Street");
        }
        
        
        
        
        // ==========================================
        // GROUP 5: OCR_IDCard (KYC-01, KYC-02, KYC-03)
        // ==========================================

        // [Fact] // KYC-01 Gửi yêu cầu nhận diện CCCD thành công
        // public async Task KYC01_RecognizeIdCard_ReturnsOk_WithData_WhenImageValid()
        // {
        //     // Arrange
        //     // 1. Giả lập file ảnh hợp lệ (Clear CCCD Image)
        //     var mockFile = new Mock<IFormFile>();
        //     mockFile.Setup(f => f.Length).Returns(1024);
        //     mockFile.Setup(f => f.FileName).Returns("cccd_clear.jpg");
        //
        //     // 2. Giả lập kết quả trả về từ Service (Extracted Data)
        //     var expectedResult = new IdCardOcrResult
        //     {
        //         IsValid = true,
        //         IdNumber = "079090001234",
        //         FullName = "NGUYEN VAN A",
        //         Dob = new DateTime(1990, 1, 1),
        //         Address = "123 Duong ABC, TP HCM",
        //         ErrorMessage = null
        //     };
        //
        //     // 3. Setup Service: Khi gọi RecognizeIdCardAsync với file bất kỳ -> Trả về kết quả trên
        //     _mockEkycService.Setup(s => s.RecognizeIdCardAsync(It.IsAny<IFormFile>()))
        //         .ReturnsAsync(expectedResult);
        //
        //     // Act
        //     // Gọi endpoint /api/ai/id-card-recognition (Action name thường là RecognizeIdCard hoặc IdCardRecognition)
        //     var result = await _controller.RecognizeIdCard(mockFile.Object);
        //
        //     // Assert
        //     // 1. Kiểm tra Status Code 200 OK
        //     var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        //     okResult.StatusCode.Should().Be(200);
        //
        //     // 2. Kiểm tra dữ liệu JSON trả về
        //     var data = okResult.Value.Should().BeOfType<IdCardOcrResult>().Subject;
        //     
        //     data.IdNumber.Should().Be("079090001234");
        //     data.Name.Should().Be("NGUYEN VAN A");
        //     data.Address.Should().Be("123 Duong ABC, TP HCM");
        // }
        
        [Fact] // KYC-02: Gửi yêu cầu xác minh thất bại - Ảnh mờ
        public async Task KYC02_SubmitVerification_ThrowsException_WhenImageInvalid()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(100);
            mockFile.Setup(f => f.FileName).Returns("blurry_image.jpg");
            // Arrange
            var request = new SubmitEkycRequest
            {
                BuyerType = BuyerType.Individual,
                FrontImage = mockFile.Object
            };

            _mockProfileService.Setup(s => s.SubmitVerificationAsync(_testUserId, request))
                .ThrowsAsync(new ApiExceptionModel(400, "INVALID_IMAGE", "Cannot detect ID card"));

            // Act & Assert
            await _controller.Invoking(c => c.SubmitVerification(request))
                .Should().ThrowAsync<ApiExceptionModel>()
                .Where(e => e.StatusCode == 400 && e.Message.Contains("Cannot detect ID card"));
        }
        
        [Fact] // KYC-03: Gửi yêu cầu thất bại - Đã xác minh rồi
        public async Task KYC03_SubmitVerification_ThrowsException_WhenAlreadyVerified()
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

        [Fact] // KYC-04: Gửi yêu cầu thất bại - Dưới 18 tuổi (Check từ Service)
        public async Task KYC04_SubmitVerification_ThrowsException_WhenUnder18()
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
        
        // ==========================================
        // GROUP 6: SubmitKYC (KYC-05)
        // ==========================================
        
        [Fact] // KYC-05: Gửi yêu cầu xác minh thành công
        public async Task KYC05_SubmitVerification_ReturnsOk_WhenSuccess()
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
        

    }
}