using FluentAssertions;
using GreenConnectPlatform.Api.Controllers;
using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GreenConnectPlatform.Tests.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IAuthService> _mockAuthService;

    public AuthControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new AuthController(_mockAuthService.Object);
    }

    // ==========================================
    // GROUP 1: LoginOrRegister (5 Cases)
    // ==========================================

    // AUTH-01: Register New User (Household) successfully -> 201 Created
    [Fact]
    public async Task AUTH01_LoginOrRegister_NewUser_ReturnsCreated()
    {
        // Arrange
        var request = new LoginOrRegisterRequest { FirebaseToken = "valid_new_user_token" };
        var authResponse = new AuthResponse
        {
            AccessToken = "access_token_new",
            User = new UserViewModel { FullName = "New User" }
        };

        // Mock service returns (Response, isNewUser: true)
        _mockAuthService.Setup(s => s.LoginOrRegisterAsync(request))
            .ReturnsAsync((authResponse, true));

        // Act
        var result = await _controller.LoginOrRegister(request);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.ActionName.Should().Be("GetMyProfile");

        var data = createdResult.Value.Should().BeOfType<AuthResponse>().Subject;
        data.AccessToken.Should().Be("access_token_new");
    }

    // AUTH-02: Login Existing User successfully -> 200 OK
    [Fact]
    public async Task AUTH02_LoginOrRegister_ExistingUser_ReturnsOk()
    {
        // Arrange
        var request = new LoginOrRegisterRequest { FirebaseToken = "valid_existing_token" };
        var authResponse = new AuthResponse
        {
            AccessToken = "access_token_existing",
            User = new UserViewModel { FullName = "Existing User" }
        };

        // Mock service returns (Response, isNewUser: false)
        _mockAuthService.Setup(s => s.LoginOrRegisterAsync(request))
            .ReturnsAsync((authResponse, false));

        // Act
        var result = await _controller.LoginOrRegister(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var data = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
        data.AccessToken.Should().Be("access_token_existing");
    }

    // AUTH-03: Login/Register fail - Invalid Token -> Throws 400 Bad Request
    [Fact]
    public async Task AUTH03_LoginOrRegister_InvalidToken_ThrowsBadRequest()
    {
        // Arrange
        var request = new LoginOrRegisterRequest { FirebaseToken = "invalid_token" };

        // Mock service throws ApiExceptionModel
        _mockAuthService.Setup(s => s.LoginOrRegisterAsync(request))
            .ThrowsAsync(new ApiExceptionModel(400, "INVALID_TOKEN", "Token verification failed"));

        // Act & Assert
        await _controller.Invoking(c => c.LoginOrRegister(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400 && e.Message == "Token verification failed");
    }

    // AUTH-04: Login/Register fail - Empty Token -> Throws 400 Bad Request
    [Fact]
    public async Task AUTH04_LoginOrRegister_EmptyToken_ThrowsBadRequest()
    {
        // Arrange
        var request = new LoginOrRegisterRequest { FirebaseToken = "" };

        _mockAuthService.Setup(s => s.LoginOrRegisterAsync(request))
            .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Token is required"));

        // Act & Assert
        await _controller.Invoking(c => c.LoginOrRegister(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }

    // AUTH-05: Login fail - Banned Account -> Throws 403 Forbidden
    [Fact]
    public async Task AUTH05_LoginOrRegister_BannedUser_ThrowsForbidden()
    {
        // Arrange
        var request = new LoginOrRegisterRequest { FirebaseToken = "banned_user_token" };

        _mockAuthService.Setup(s => s.LoginOrRegisterAsync(request))
            .ThrowsAsync(new ApiExceptionModel(403, "ACCOUNT_BANNED", "Account is banned"));

        // Act & Assert
        await _controller.Invoking(c => c.LoginOrRegister(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 403);
    }

    // ==========================================
    // GROUP 2: AdminLogin (4 Cases)
    // ==========================================

    // AUTH-06: Admin Login successfully -> 200 OK
    [Fact]
    public async Task AUTH06_AdminLogin_Success_ReturnsOk()
    {
        // Arrange
        var request = new AdminLoginRequest { Email = "admin@test.com", Password = "123" };
        var authResponse = new AuthResponse { AccessToken = "admin_token" };

        _mockAuthService.Setup(s => s.AdminLoginAsync(request))
            .ReturnsAsync(authResponse);

        // Act
        var result = await _controller.AdminLogin(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);

        var data = okResult.Value.Should().BeOfType<AuthResponse>().Subject;
        data.AccessToken.Should().Be("admin_token");
    }

    // AUTH-07: Admin Login fail - Wrong Password -> Throws 401 Unauthorized (or 400)
    [Fact]
    public async Task AUTH07_AdminLogin_WrongPassword_ThrowsUnauthorized()
    {
        // Arrange
        var request = new AdminLoginRequest { Email = "admin@test.com", Password = "wrong" };

        _mockAuthService.Setup(s => s.AdminLoginAsync(request))
            .ThrowsAsync(new ApiExceptionModel(401, "AUTH_FAILED", "Invalid credentials"));

        // Act & Assert
        await _controller.Invoking(c => c.AdminLogin(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 401);
    }

    // AUTH-08: Admin Login fail - User Not Found -> Throws 404 Not Found
    [Fact]
    public async Task AUTH08_AdminLogin_UserNotFound_ThrowsNotFound()
    {
        // Arrange
        var request = new AdminLoginRequest { Email = "unknown@test.com", Password = "123" };

        _mockAuthService.Setup(s => s.AdminLoginAsync(request))
            .ThrowsAsync(new ApiExceptionModel(404, "NOT_FOUND", "User not found"));

        // Act & Assert
        await _controller.Invoking(c => c.AdminLogin(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 404);
    }

    // AUTH-09: Admin Login fail - Missing Credentials -> Throws 400 Bad Request
    [Fact]
    public async Task AUTH09_AdminLogin_MissingCredentials_ThrowsBadRequest()
    {
        // Arrange
        var request = new AdminLoginRequest { Email = "", Password = "" };

        _mockAuthService.Setup(s => s.AdminLoginAsync(request))
            .ThrowsAsync(new ApiExceptionModel(400, "VALIDATION_ERROR", "Email and Password are required"));

        // Act & Assert
        await _controller.Invoking(c => c.AdminLogin(request))
            .Should().ThrowAsync<ApiExceptionModel>()
            .Where(e => e.StatusCode == 400);
    }
}