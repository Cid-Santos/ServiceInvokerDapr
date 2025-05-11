using Messaging.Core.Contracts;
using Messaging.Core.Models;
using Messaging.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Infrastructure.Repositories
{
    // MessageRepository.cs  
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDbContext _context;

        public MessageRepository(MessageDbContext context)
        {
            _context = context;
        }

        public async Task LogMessageAsync(MessageLog log)
        {
            _context.MessageLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageLog>> GetLogsAsync(DateTime from, DateTime to)
        {
            return await _context.MessageLogs
                .Where(x => x.SentAt >= from && x.SentAt <= to)
                .OrderByDescending(x => x.SentAt)
                .ToListAsync();
        }
    }
}

