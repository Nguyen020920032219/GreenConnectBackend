using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Users;

public interface IUserRepository : IBaseRepository<User, Guid>
{
    Task<(List<User> Items,Dictionary<Guid, List<string>> RolesMap, int TotalCount)> GetUsersAsync(
        int pageIndex,
        int pageSize,
        Guid? roleId,
        string? fullName);
    Task<User?> GetUserByIdAsync(Guid userId);
    
    Task<List<User>> GetUsersFroReport(DateTime startDate, DateTime endDate);
}