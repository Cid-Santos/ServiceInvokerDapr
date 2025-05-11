using Messaging.Core.Models;

namespace Messaging.Core.Contracts
{
    // IMessageRepository.cs
    public interface IMessageRepository
    {
        Task LogMessageAsync(MessageLog log);
        Task<IEnumerable<MessageLog>> GetLogsAsync(DateTime from, DateTime to);
    }
}
