using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.UserDevices;

public interface IUserDeviceRepository : IBaseRepository<UserDevice, Guid>
{
    Task<UserDevice?> GetByTokenAsync(string fcmToken);
    Task<List<string>> GetTokensByUserIdAsync(Guid userId);
}