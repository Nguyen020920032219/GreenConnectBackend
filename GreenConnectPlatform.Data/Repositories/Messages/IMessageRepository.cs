using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Messages;

public interface IMessageRepository : IBaseRepository<Message, Guid>
{
    Task<(List<Message> Items, int TotalCount)> GetMessagesByRoomIdAsync(Guid chatRoomId, int pageIndex, int pageSize);
    Task<List<Message>> GetAllMessageUnRead(Guid chatRoomId, Guid userId);
}