using FirebaseAdmin.Auth;
using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Profiles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
// Model Exception của bạn

namespace GreenConnectPlatform.Business.Services.Auth;

public class AuthService : IAuthService
{
    private readonly FirebaseAuth _firebaseAuth;
    private readonly IJwtService _jwtService;
    private readonly IProfileRepository _profileRepository;
    private readonly UserManager<User> _userManager;

    public AuthService(
        UserManager<User> userManager,
        IProfileRepository profileRepository,
        IJwtService jwtService,
        FirebaseAuth firebaseAuth)
    {
        _userManager = userManager;
        _profileRepository = profileRepository;
        _jwtService = jwtService;
        _firebaseAuth = firebaseAuth;
    }

    public async Task<(AuthResponse Response, bool IsNewUser)> LoginOrRegisterAsync(LoginOrRegisterRequest request)
    {
        // 1. Verify Firebase Token
        string phoneNumber;
        try
        {
            var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(request.FirebaseToken);
            phoneNumber = decodedToken.Claims.GetValueOrDefault("phone_number")?.ToString() ?? "";

            if (string.IsNullOrEmpty(phoneNumber))
                // Dùng mã lỗi "400" thay vì chuỗi in hoa
                throw new ApiExceptionModel(StatusCodes.Status400BadRequest, "400",
                    "Firebase Token không chứa số điện thoại.");
        }
        catch (Exception ex)
        {
            throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401",
                $"Lỗi xác thực Firebase: {ex.Message}");
        }

        // 2. Tìm User
        var user = await _userManager.FindByNameAsync(phoneNumber);
        var isNewUser = user == null;

        if (isNewUser) user = await CreateHouseholdUserAsync(phoneNumber);

        // 3. Validate Trạng thái User (Pending/Blocked)
        await ValidateUserStatusAsync(user!);

        // 4. Tạo Token & Response
        var response = await GenerateAuthResponseAsync(user!);
        return (response, isNewUser);
    }

    public async Task<AuthResponse> AdminLoginAsync(AdminLoginRequest request)
    {
        User? user;
        var normalizedEmail = request.Email.ToUpper();
        var isDevLogin = false;

        // --- DEV BACKDOOR (Giữ lại cho ae test) ---
        var testAccounts = new List<string> { "CHITU@GC.COM", "ANHBA@GC.COM", "VUAABC@GC.COM" };

        if (testAccounts.Contains(normalizedEmail) && request.Password == "@1")
        {
            user = await _userManager.FindByEmailAsync(request.Email);
            isDevLogin = true;
        }
        else
        {
            // Login thật
            user = await _userManager.FindByEmailAsync(request.Email)
                   ?? await _userManager.FindByNameAsync(request.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new ApiExceptionModel(StatusCodes.Status401Unauthorized, "401",
                    "Email hoặc mật khẩu không chính xác.");
        }

        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Tài khoản không tồn tại.");

        if (user.Status == UserStatus.Blocked)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Tài khoản đã bị khóa.");

        // Check Role Admin (Chỉ check nếu không phải Dev Login)
        var roles = await _userManager.GetRolesAsync(user);
        if (!isDevLogin && !roles.Contains("Admin"))
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403",
                "Bạn không có quyền truy cập trang quản trị.");

        return await GenerateAuthResponseAsync(user);
    }

    // ================= HELPER METHODS =================

    private async Task<User> CreateHouseholdUserAsync(string phoneNumber)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = phoneNumber,
            NormalizedUserName = phoneNumber.ToUpper(),
            Email = $"{phoneNumber}@greenconnect.local",
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            BuyerType = null
        };

        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApiExceptionModel(StatusCodes.Status500InternalServerError, "500", $"Lỗi tạo User: {errors}");
        }

        await _userManager.AddToRoleAsync(newUser, "Household");

        // Tạo Profile mặc định (Dùng Repository chuẩn)
        var newProfile = new Data.Entities.Profile
        {
            ProfileId = Guid.NewGuid(),
            UserId = newUser.Id,
            PointBalance = 200, // Điểm thưởng
            RankId = 1 // Rank mặc định
        };

        await _profileRepository.AddAsync(newProfile);

        return newUser;
    }

    private async Task ValidateUserStatusAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        // Nếu là Collector mà chưa được duyệt -> Chặn
        if (!roles.Contains("Household") && user.Status == UserStatus.PendingVerification)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Tài khoản đang chờ Admin xác minh.");

        if (user.Status == UserStatus.Blocked)
            throw new ApiExceptionModel(StatusCodes.Status403Forbidden, "403", "Tài khoản đã bị khóa.");
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = await _jwtService.GenerateAccessTokenAsync(user, roles);

        // Lấy Profile bằng Repository
        var profiles = await _profileRepository.FindAsync(p => p.UserId == user.Id);
        var profile = profiles.FirstOrDefault();

        // Logic hiển thị tên Rank
        var rankName = "Bronze";
        if (profile != null && profile.RankId == 2) rankName = "Silver";
        if (profile != null && profile.RankId == 3) rankName = "Gold";

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            FullName = user.FullName ?? "Người dùng mới",
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = profile?.AvatarUrl,
            PointBalance = profile?.PointBalance ?? 0,
            Rank = rankName,
            Roles = roles
        };

        return new AuthResponse
        {
            AccessToken = accessToken,
            User = userViewModel
        };
    }
}