using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GreenConnectPlatform.Business.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> GenerateAccessTokenAsync(User user, IList<string> roles)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"] ?? throw new ApiExceptionModel(StatusCodes.Status500InternalServerError,
            "500", "Chưa cấu hình JWT Key trong appsettings.");
        ;
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        if (!double.TryParse(jwtSettings["AccessTokenExpiryInMinutes"], out var expiryMinutes))
            expiryMinutes = 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? "Unknown"),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            new("BuyerType", user.BuyerType?.ToString() ?? "")
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.Now.AddMinutes(expiryMinutes),
            signingCredentials: creds
        );

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}