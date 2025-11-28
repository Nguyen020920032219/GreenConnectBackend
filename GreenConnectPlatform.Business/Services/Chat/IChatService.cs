using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Models.Paging;

namespace GreenConnectPlatform.Business.Services.Chat;

public interface IChatService
{
    Task<MessageModel> SendMessageAsync(Guid senderId, SendMessageModel model);

    Task<PaginatedResult<MessageModel>> GetChatHistoryAsync(int pageIndex, int pageSize, Guid chatRoomId);

    Task<PaginatedResult<ChatRoomModel>> GetMyChatRoomAsync(Guid userId, int pageIndex, int pageSize);

    Task MarkAllAsReadAsync(Guid chatRoomId, Guid userId);
}