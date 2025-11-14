using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FirebaseAdmin.Auth;
using System.Security.Claims;
using GreenConnectPlatform.Business.Models.Auth;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Business.Services.Auth;
using GreenConnectPlatform.Business.Services.Jwt;
using GreenConnectPlatform.Data.Repositories.Profiles;

namespace GreenConnectPlatform.Business.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IProfileRepository _profileRepository;
    private readonly IJwtService _jwtService;
    private readonly FirebaseAuth _firebaseAuth;

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

    public async Task<AuthResponse> LoginOrRegisterAsync(LoginOrRegisterRequest request)
    {
        FirebaseToken decodedToken;
        try
        {
            decodedToken = await _firebaseAuth.VerifyIdTokenAsync(request.FirebaseToken);
        }
        catch (Exception ex)
        {
            throw new UnauthorizedAccessException("Invalid Firebase Token. " + ex.Message);
        }

        var phoneNumber = decodedToken.Claims.GetValueOrDefault("phone_number")?.ToString();
        if (string.IsNullOrEmpty(phoneNumber))
        {
            throw new ApplicationException("Firebase token did not contain a phone number.");
        }

        var user = await _userManager.Users
            .Include(u => u.Profile)
            .ThenInclude(p => p.Rank)
            .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);

        bool isNewUser = false;

        if (user == null)
        {
            isNewUser = true;
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = phoneNumber,
                NormalizedUserName = phoneNumber,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = true,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                BuyerType = null
            };

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                throw new ApplicationException(
                    $"Failed to create new user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(newUser, "Household");

            var newProfile = new Profile
            {
                ProfileId = Guid.NewGuid(),
                UserId = newUser.Id,
                PointBalance = 200,
                RankId = 1
            };
            await _profileRepository.Add(newProfile);

            user = newUser;
            user.Profile = newProfile;
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Contains("IndividualCollector") || roles.Contains("BusinessCollector"))
        {
            if (user.Status == UserStatus.PendingVerification)
            {
                throw new UnauthorizedAccessException("Your account is  pending verification.");
            }

            if (user.Status == UserStatus.Blocked)
            {
                throw new UnauthorizedAccessException("Your account is blocked.");
            }
        }

        var accessToken = await _jwtService.CreateTokenAsync(user, roles);

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.Profile?.AvatarUrl,
            PointBalance = user.Profile?.PointBalance ?? 200,
            Rank = user.Profile?.Rank?.Name ?? "Bronze",
            Roles = roles
        };

        return new AuthResponse
        {
            AccessToken = accessToken,
            User = userViewModel
        };
    }
}