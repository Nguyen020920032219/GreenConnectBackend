using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Entities;

namespace GreenConnectPlatform.Business.Services.Users;

public interface IUserService
{
    Task<PaginatedResult<UserModel>> GetUsersAsync(
        int pageIndex,
        int pageSize,
        Guid? roleId,
        string? fullName);
    
    Task BanOrUnbanUserAsync(Guid userId);
}