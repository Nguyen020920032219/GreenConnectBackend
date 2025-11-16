using GreenConnectPlatform.Data.Entities;

namespace GreenConnectPlatform.Business.Services.Jwt;

public interface IJwtService
{
    Task<string> CreateTokenAsync(User user, IList<string> roles);
}