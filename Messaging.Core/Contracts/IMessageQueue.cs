// Em Messaging.Core/Contracts/IMessageQueue.cs
using System.Threading;
using System.Threading.Tasks;
using Messaging.Core.Models;

namespace Messaging.Core.Contracts
{
    public interface IMessageQueue
    {
        Task EnqueueAsync(EmailMessage message);
        Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken);
    }
}
