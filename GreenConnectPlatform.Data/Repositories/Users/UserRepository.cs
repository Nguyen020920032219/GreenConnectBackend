using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Users;

public class UserRepository : BaseRepository<GreenConnectDbContext, User, Guid>, IUserRepository
{
    public UserRepository(GreenConnectDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<User> Items,Dictionary<Guid, List<string>> RolesMap, int TotalCount)> GetUsersAsync(int pageIndex, int pageSize, Guid? roleId, string? fullName)
    {
        var query = _dbSet.AsNoTracking() ;
        if (roleId != null)
        {
            var userIdInRole = _context.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .Select(ur => ur.UserId);
            query = query.Where(u => userIdInRole.Contains(u.Id));
        }
        if (!string.IsNullOrEmpty(fullName))
        {
            query = query.Where(u => u.FullName.ToLower().Contains(fullName.ToLower()));
        }
        var totalCount = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.FullName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var userIds = users.Select(u => u.Id).ToList();
        var rawRoles = await (from ur in _context.UserRoles
            join r in _context.Roles on ur.RoleId equals r.Id
            where userIds.Contains(ur.UserId)
            select new 
            { 
                ur.UserId, 
                RoleName = r.Name 
            }).ToListAsync();
        var rolesMap = rawRoles
            .GroupBy(x => x.UserId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.RoleName).ToList());
        return (users, rolesMap, totalCount);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<List<User>> GetUsersFroReport(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate)
            .ToListAsync();
    }
}