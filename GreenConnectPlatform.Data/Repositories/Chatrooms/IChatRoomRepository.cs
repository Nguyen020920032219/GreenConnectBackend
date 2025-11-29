using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;

namespace GreenConnectPlatform.Data.Repositories.Chatrooms;

public interface IChatRoomRepository : IBaseRepository<ChatRoom, Guid>
{
    Task<ChatRoom?> GetChatRoomByTransactionId(Guid transactionId);

    Task<(List<ChatRoom>Items, int TotalCount)> GetChatRooms(Guid userId, string? name, int pageIndex, int pageSize);
}