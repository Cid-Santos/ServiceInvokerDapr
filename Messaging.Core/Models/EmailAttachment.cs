namespace Messaging.Core.Models
{
    // EmailAttachment.cs
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}
