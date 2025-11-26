using Microsoft.AspNetCore.SignalR;

namespace GreenConnectPlatform.Business.Hubs;

public class ChatHub : Hub
{
    public async Task JoinChatRoom(string transactionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, transactionId);
    }

    public async Task LeaveChatRoom(string transactionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, transactionId);
    }
}