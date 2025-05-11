using Messaging.Core.Contracts;
using Messaging.Core.Enums;
using Messaging.Core.Exceptions;
using Messaging.Core.Models;
using Messaging.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Messaging.Infrastructure.Services
{
    // SmtpEmailService.cs
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly IMessageRepository _repository;

        public SmtpEmailService(
            IOptions<SmtpSettings> settings,
            ILogger<SmtpEmailService> logger,
            IMessageRepository repository)
        {
            _settings = settings.Value;
            _logger = logger;
            _repository = repository;
        }

        public async Task SendAsync(EmailMessage message)
        {
            try
            {
                using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
                {
                    EnableSsl = _settings.EnableSsl,
                    Credentials = new NetworkCredential(_settings.Username, _settings.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.FromEmail, _settings.FromName),
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = message.IsHtml
                };

                mailMessage.To.Add(message.To);

                foreach (var attachment in message.Attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(
                        new MemoryStream(attachment.Content),
                        attachment.FileName,
                        attachment.ContentType));
                }

                await smtpClient.SendMailAsync(mailMessage);

                await _repository.LogMessageAsync(new MessageLog
                {
                    Id = Guid.NewGuid(),
                    Recipient = message.To,
                    Subject = message.Subject,
                    SentAt = DateTime.UtcNow,
                    Status = MessageStatus.Sent
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", message.To);

                await _repository.LogMessageAsync(new MessageLog
                {
                    Id = Guid.NewGuid(),
                    Recipient = message.To,
                    Subject = message.Subject,
                    SentAt = DateTime.UtcNow,
                    Status = MessageStatus.Failed,
                    ErrorMessage = ex.Message
                });

                throw new MessageDeliveryException("Email delivery failed", ex);
            }
        }

        public async Task SendBulkAsync(IEnumerable<EmailMessage> messages)
        {
            // Implementação paralelizada
            await Parallel.ForEachAsync(messages, async (message, _) =>
            {
                await SendAsync(message);
            });
        }
    }
}
