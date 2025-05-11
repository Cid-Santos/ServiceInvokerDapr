using Messaging.Core.Models;

namespace Messaging.Core.Contracts
{
    // IEmailService.cs
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message);
        Task SendBulkAsync(IEnumerable<EmailMessage> messages);
    }
}
