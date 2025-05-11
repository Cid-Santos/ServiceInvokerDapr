using Messaging.Api.DTOs.Requests;
using Messaging.Core.Contracts;
using Messaging.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Controllers
{
    // MessagesController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MessagesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("email")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            var message = new EmailMessage
            {
                To = request.To,
                Subject = request.Subject,
                Body = request.Body,
                IsHtml = request.IsHtml
            };

            await _emailService.SendAsync(message);

            return Accepted(new { Status = "Email sent successfully" });
        }
    }
}
