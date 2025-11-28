using GreenConnectPlatform.Business.Hubs;
using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Services.FileStorage;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Chatrooms;
using GreenConnectPlatform.Data.Repositories.Messages;
using GreenConnectPlatform.Data.Repositories.Transactions;
using GreenConnectPlatform.Data.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace GreenConnectPlatform.Business.Services.Chat;

public class ChatService : IChatService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IMessageRepository _messageRepository;
    private readonly IChatRoomRepository _roomRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    public ChatService(IChatRoomRepository roomRepository,
        IMessageRepository messageRepository,
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IHubContext<ChatHub> hubContext,
        IFileStorageService fileStorageService)
    {
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
        _fileStorageService = fileStorageService;
    }

    public async Task<MessageModel> SendMessageAsync(Guid senderId, SendMessageModel model)
    {
        var room = await _roomRepository.GetChatRoomByTransactionId(model.TransactionId);
        if (room == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                "Không tìm thấy phòng chat cho giao dịch này");

        var message = new Message
        {
            MessageId = Guid.NewGuid(),
            ChatRoomId = room.ChatRoomId,
            SenderId = senderId,
            Content = model.Content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };
        await _messageRepository.AddAsync(message);
        var user = await _userRepository.GetUserByIdAsync(senderId);
        var messageModel = new MessageModel
        {
            MessageId = message.MessageId,
            SenderId = message.SenderId,
            Content = message.Content,
            Timestamp = message.Timestamp,
            IsRead = message.IsRead,
            SenderName = user.FullName,
            SenderAvatar = user.Profile.AvatarUrl != null
                ? await _fileStorageService.GetReadSignedUrlAsync(user.Profile.AvatarUrl)
                : null
        };

        await _hubContext.Clients.Group(model.TransactionId.ToString())
            .SendAsync("ReceiveMessage", messageModel);

        var participants = room.ChatParticipants;

        if (participants == null || !participants.Any())
        {
            var trans = await _transactionRepository.GetByIdWithDetailsAsync(model.TransactionId);
            if (trans != null)
                participants = new List<ChatParticipant>
                {
                    new() { UserId = trans.HouseholdId },
                    new() { UserId = trans.ScrapCollectorId }
                };
        }

        if (participants != null)
            foreach (var participant in participants)
            {
                var partnerId = participants.FirstOrDefault(p => p.UserId != participant.UserId)?.UserId;
                if (partnerId == null) continue;

                var partnerUser = await _userRepository.GetUserByIdAsync(partnerId.Value);

                var unreadCount = participant.UserId != senderId ? 1 : 0;

                var chatListItem = new ChatRoomModel
                {
                    ChatRoomId = room.ChatRoomId,
                    TransactionId = room.TransactionId,
                    LastMessage = message.Content,
                    LastMessageTime = message.Timestamp,
                    UnreadCount = unreadCount,

                    PartnerName = partnerUser?.FullName ?? "Người dùng",
                    PartnerAvatar = partnerUser?.Profile?.AvatarUrl != null
                        ? await _fileStorageService.GetReadSignedUrlAsync(partnerUser.Profile?.AvatarUrl)
                        : null
                };

                var groupName = $"User_{participant.UserId.ToString().ToLower()}";

                await _hubContext.Clients.Group(groupName)
                    .SendAsync("UpdateChatList", chatListItem);
            }

        return messageModel;
    }

    public async Task<PaginatedResult<MessageModel>> GetChatHistoryAsync(int pageIndex, int pageSize, Guid chatRoomId)
    {
        var (items, totalCount) = await _messageRepository.GetMessagesByRoomIdAsync(chatRoomId, pageIndex, pageSize);
        var tasks = items.Select(async m => new MessageModel
        {
            MessageId = m.MessageId,
            SenderId = m.SenderId,
            Content = m.Content,
            Timestamp = m.Timestamp,
            IsRead = m.IsRead,
            SenderName = m.Sender.FullName ?? "Unknown",
            SenderAvatar = m.Sender?.Profile?.AvatarUrl != null
                ? await _fileStorageService.GetReadSignedUrlAsync(m.Sender.Profile?.AvatarUrl)
                : null
        });
        var messageDto = (await Task.WhenAll(tasks)).ToList();
        return new PaginatedResult<MessageModel>
        {
            Data = messageDto,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task<PaginatedResult<ChatRoomModel>> GetMyChatRoomAsync(Guid userId, int pageIndex, int pageSize)
    {
        var (items, totalCount) = await _roomRepository.GetChatRooms(userId, pageIndex, pageSize);
        var tasks = items.Select(async room =>
        {
            var partner = room.ChatParticipants.FirstOrDefault(p => p.UserId != userId)?.User;
            var lastMessage = room.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault();
            var unreadCount = room.Messages.Count(m => !m.IsRead && m.SenderId != userId);
            string? avatarUrl = null;
            return new ChatRoomModel
            {
                ChatRoomId = room.ChatRoomId,
                TransactionId = room.TransactionId,
                PartnerName = partner?.FullName ?? "Người dùng",
                PartnerAvatar = partner?.Profile?.AvatarUrl != null
                    ? await _fileStorageService.GetReadSignedUrlAsync(partner?.Profile?.AvatarUrl)
                    : null,
                LastMessage = lastMessage?.Content ?? "",
                LastMessageTime = lastMessage?.Timestamp,
                UnreadCount = unreadCount
            };
        });
        var data = (await Task.WhenAll(tasks)).ToList();
        return new PaginatedResult<ChatRoomModel>
        {
            Data = data,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task MarkAllAsReadAsync(Guid chatRoomId, Guid userId)
    {
        var messages = await _messageRepository.GetAllMessageUnRead(chatRoomId, userId);
        if (messages.Any())
            foreach (var message in messages)
            {
                message.IsRead = true;
                await _messageRepository.UpdateAsync(message);
            }
    }
}