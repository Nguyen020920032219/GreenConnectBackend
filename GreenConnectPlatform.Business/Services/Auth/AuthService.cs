using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FirebaseAdmin.Auth;
using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GreenConnectPlatform.Business.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly FirebaseService _firebase;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AuthService(UserManager<User> userManager, IConfiguration configuration, FirebaseService firebase,
        IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _firebase = firebase;
        _mapper = mapper;
    }

    public async Task<AuthResultModel> LoginWithFirebaseAsync(FirebaseLoginRequestModel request)
    {
        var (decodedToken, error) = await VerifyFirebaseTokenAndGetPhone(request.IdToken);
        if (error != null) return error;

        var phoneNumber = decodedToken!.Claims["phone_number"].ToString()!;
        var user = await _userManager.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

        if (user == null)
        {
            user = new User
            {
                FullName = "Người dùng mới",
                UserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };
            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
                return new AuthResultModel
                    { Success = false, Errors = createResult.Errors.Select(e => e.Description).ToList() };

            await _userManager.AddToRoleAsync(user, "Household");
        }

        return await GenerateSuccessfulAuthResult(user);
    }

    public async Task<AuthResultModel> AdminLoginAsync(AdminLoginRequestModel request)
    {
        var testAccounts = new List<string>() { "household@gmail.com", "collector@gmail.com", "scrapyard@gmail.com" };

        if (testAccounts.Contains(request.Email) && request.Password == "@1")
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return new AuthResultModel { Success = false, Errors = ["Email hoặc mật khẩu không đúng."] };

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return new AuthResultModel { Success = false, Errors = ["Email hoặc mật khẩu không đúng."] };

            var token = await GenerateJwtToken(user);
            var userModel = await CreateUserModel(user);
            return new AuthResultModel { Success = true, Token = token, User = userModel };
        }
        else
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
                return new AuthResultModel { Success = false, Errors = ["Email hoặc mật khẩu không đúng."] };

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return new AuthResultModel { Success = false, Errors = ["Email hoặc mật khẩu không đúng."] };

            var token = await GenerateJwtToken(user);
            var userModel = await CreateUserModel(user);
            return new AuthResultModel { Success = true, Token = token, User = userModel };
        }
    }

    private async Task<(FirebaseToken? DecodedToken, AuthResultModel? Error)> VerifyFirebaseTokenAndGetPhone(
        string idToken)
    {
        var decoded = await _firebase.VerifyFirebaseTokenAsync(idToken);
        if (decoded == null)
            return (null, new AuthResultModel { Success = false, Errors = ["Token không hợp lệ hoặc đã hết hạn."] });

        if (!decoded.Claims.ContainsKey("phone_number") ||
            string.IsNullOrEmpty(decoded.Claims["phone_number"]?.ToString()))
            return (null,
                new AuthResultModel { Success = false, Errors = ["Không tìm thấy số điện thoại trong token."] });

        return (decoded, null);
    }

    private async Task<AuthResultModel> GenerateSuccessfulAuthResult(User user)
    {
        var token = await GenerateJwtToken(user);
        var userModel = await CreateUserModel(user);
        return new AuthResultModel { Success = true, Token = token, User = userModel };
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Missing JWT Key");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty)
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            jwtSettings["Issuer"],
            jwtSettings["Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpiryInMinutes"])),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<UserModel> CreateUserModel(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new UserModel
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.Profile?.AvatarUrl,
            Roles = roles.ToList()
        };
    }
}