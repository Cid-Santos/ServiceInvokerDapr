namespace Messaging.Api.DTOs.Responses
{
    // MessageResponse.cs
    public class MessageResponse
    {
        public Guid MessageId { get; set; }
        public string Status { get; set; }
        public DateTime SentAt { get; set; }
    }
}
