namespace Messaging.Core.Models
{
    // EmailMessage.cs
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public List<EmailAttachment> Attachments { get; set; } = new();
    }
}
