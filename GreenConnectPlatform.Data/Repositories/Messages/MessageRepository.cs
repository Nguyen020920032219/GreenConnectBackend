using GreenConnectPlatform.Data.Configurations;
using GreenConnectPlatform.Data.Entities;
using GreenConnectPlatform.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace GreenConnectPlatform.Data.Repositories.Messages;

public class MessageRepository : BaseRepository<GreenConnectDbContext, Message, Guid>, IMessageRepository
{
    public MessageRepository(GreenConnectDbContext context) : base(context)
    {
    }


    public async Task<(List<Message> Items, int TotalCount)> GetMessagesByRoomIdAsync(Guid chatRoomId, int pageIndex,
        int pageSize)
    {
        var query = _dbSet
            .AsQueryable()
            .Where(m => m.ChatRoomId == chatRoomId);
        var totalCount = await query.CountAsync();
        var items = await query
            .Include(m => m.Sender).ThenInclude(s => s.Profile)
            .OrderByDescending(m => m.Timestamp)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<List<Message>> GetAllMessageUnRead(Guid chatRoomId, Guid userId)
    {
        return await _dbSet.Where(m => m.ChatRoomId == chatRoomId
                                       && m.SenderId != userId
                                       && !m.IsRead)
            .ToListAsync();
    }
}