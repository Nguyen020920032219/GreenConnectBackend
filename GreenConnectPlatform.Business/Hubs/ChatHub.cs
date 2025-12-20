using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GreenConnectPlatform.Business.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinChatRoom(string transactionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, transactionId);
    }

    public async Task JoinUserTopic(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId.ToLower()}");
    }

    public async Task LeaveChatRoom(string transactionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, transactionId);
    }
}