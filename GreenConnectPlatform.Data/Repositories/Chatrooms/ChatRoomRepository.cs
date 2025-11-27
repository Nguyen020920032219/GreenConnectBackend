using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Chatrooms;

public class ChatRoomRepository : BaseRepository<GreenConnectDbContext, ChatRoom, Guid>, IChatRoomRepository
{
    public ChatRoomRepository(GreenConnectDbContext context) : base(context)
    {
    }

    public async Task<ChatRoom?> GetChatRoomByTransactionId(Guid transactionId)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(r => r.ChatParticipants)
            .ThenInclude(cp => cp.User)   
            .ThenInclude(u => u.Profile)  
            .Include(r => r.Messages)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.TransactionId == transactionId);
    }

    public async Task<(List<ChatRoom> Items, int TotalCount)> GetChatRooms(Guid userId, int pageIndex, int pageSize)
    {
        var query = _dbSet
            .AsNoTracking()
            .Where(c => c.ChatParticipants.Any(p => p.UserId == userId));
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(c => c.ChatParticipants)
            .ThenInclude(p => p.User)
            .ThenInclude(u => u.Profile)
            .Include(c => c.Messages)
            .OrderByDescending(c => c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault().Timestamp)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsSplitQuery()
            .ToListAsync();
        return (items, totalCount);
    }
}