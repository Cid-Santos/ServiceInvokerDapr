using Messaging.Core.Enums;

namespace Messaging.Core.Models
{
    // MessageLog.cs
    public class MessageLog
    {
        public Guid Id { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public DateTime SentAt { get; set; }
        public MessageStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
