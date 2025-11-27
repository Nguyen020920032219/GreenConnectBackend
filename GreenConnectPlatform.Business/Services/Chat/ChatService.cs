using GreenConnectPlatform.Business.Hubs;
using GreenConnectPlatform.Business.Models.Chat;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
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
    private readonly IChatRoomRepository _roomRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatService(IChatRoomRepository roomRepository, 
        IMessageRepository messageRepository, 
        ITransactionRepository transactionRepository, 
        IUserRepository userRepository,
        IHubContext<ChatHub> hubContext)
    {
        _roomRepository = roomRepository;
        _messageRepository = messageRepository;
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _hubContext = hubContext;
    }
    
    public async Task<MessageModel> SendMessageAsync(Guid senderId,SendMessageModel model)
    {
        var room = await _roomRepository.GetChatRoomByTransactionId(model.TransactionId);
        if (room == null)
        {
            var transaction = await _transactionRepository.GetByIdWithDetailsAsync(model.TransactionId);
            if(transaction == null)
                throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404",
                    "Không tìm thấy giao dịch này");
            room = new ChatRoom
            {
                ChatRoomId = Guid.NewGuid(),
                TransactionId = model.TransactionId,
                CreatedAt = DateTime.UtcNow,
                ChatParticipants = new List<ChatParticipant>()
            };
            
            room.ChatParticipants.Add(new ChatParticipant
            {
                UserId = transaction.HouseholdId,
                ChatRoomId = room.ChatRoomId,
                JoinedAt = DateTime.UtcNow
            });
            room.ChatParticipants.Add(new ChatParticipant
            {
                UserId = transaction.ScrapCollectorId,
                ChatRoomId = room.ChatRoomId,
                JoinedAt = DateTime.UtcNow
            });
            await _roomRepository.AddAsync(room);
        }

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
            SenderAvatar = user.Profile.AvatarUrl
        };

        await _hubContext.Clients.Group(model.TransactionId.ToString())
            .SendAsync("ReceiveMessage", messageModel);

        var participants = room.ChatParticipants;
        
        if (participants == null || !participants.Any())
        {
            var trans = await _transactionRepository.GetByIdWithDetailsAsync(model.TransactionId);
            if(trans != null) {
                participants = new List<ChatParticipant> {
                    new ChatParticipant { UserId = trans.HouseholdId },
                    new ChatParticipant { UserId = trans.ScrapCollectorId }
                };
            }
        }

        if (participants != null)
        {
            foreach (var participant in participants)
            {
                var partnerId = participants.FirstOrDefault(p => p.UserId != participant.UserId)?.UserId;
                if (partnerId == null) continue;

                var partnerUser = await _userRepository.GetUserByIdAsync(partnerId.Value);

                int unreadCount = (participant.UserId != senderId) ? 1 : 0;

                var chatListItem = new ChatRoomModel
                {
                    ChatRoomId = room.ChatRoomId,
                    TransactionId = room.TransactionId,
                    LastMessage = message.Content,
                    LastMessageTime = message.Timestamp,
                    UnreadCount = unreadCount,
                
                    PartnerName = partnerUser?.FullName ?? "Người dùng",
                    PartnerAvatar = partnerUser?.Profile?.AvatarUrl ?? ""
                };

                string groupName = $"User_{participant.UserId.ToString().ToLower()}";

                await _hubContext.Clients.Group(groupName)
                    .SendAsync("UpdateChatList", chatListItem);
            }
        }
        return messageModel;
    }

    public async Task<PaginatedResult<MessageModel>> GetChatHistoryAsync(int pageIndex, int pageSize, Guid chatRoomId)
    {
        var (items, totalCount) = await _messageRepository.GetMessagesByRoomIdAsync(chatRoomId, pageIndex, pageSize);

        var messageDto = items.Select(m => new MessageModel
        {
            MessageId = m.MessageId,
            SenderId = m.SenderId,
            Content = m.Content,
            Timestamp = m.Timestamp,
            IsRead = m.IsRead,
            SenderName = m.Sender?.FullName ?? "Unknown",
            SenderAvatar = m.Sender?.Profile?.AvatarUrl ?? ""
        }).ToList();

        return new PaginatedResult<MessageModel>
        {
            Data = messageDto,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task<PaginatedResult<ChatRoomModel>> GetMyChatRoomAsync(Guid userId, int pageIndex, int pageSize)
    {
        var (items, totalCount) = await _roomRepository.GetChatRooms(userId, pageIndex, pageSize);
        var data = items.Select(room =>
        {
            var partner = room.ChatParticipants.FirstOrDefault(p => p.UserId != userId)?.User;
            var lastMessage = room.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault();
            var unreadCount = room.Messages.Count(m => !m.IsRead && m.SenderId != userId);
            return new ChatRoomModel
            {
                ChatRoomId = room.ChatRoomId,
                TransactionId = room.TransactionId,
                PartnerName = partner?.FullName ?? "Người dùng",
                PartnerAvatar = partner?.Profile?.AvatarUrl ?? "",
                LastMessage = lastMessage?.Content ?? "",
                LastMessageTime = lastMessage?.Timestamp,
                UnreadCount = unreadCount
            };
        }).ToList();
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
        {
            foreach (var message in messages)
            {
                message.IsRead = true;
                await _messageRepository.UpdateAsync(message);
            }
        }
    }
}