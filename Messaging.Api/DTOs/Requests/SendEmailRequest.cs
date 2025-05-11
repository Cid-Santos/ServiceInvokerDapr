using System.ComponentModel.DataAnnotations;

namespace Messaging.Api.DTOs.Requests
{
    // SendEmailRequest.cs
    public class SendEmailRequest
    {
        [Required, EmailAddress]
        public string To { get; set; }

        [Required, MaxLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public bool IsHtml { get; set; } = true;
    }
}
