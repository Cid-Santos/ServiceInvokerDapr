using Messaging.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messaging.Worker.Workers
{
    // EmailQueueWorker.cs
    public class EmailQueueWorker : BackgroundService
    {
        private readonly ILogger<EmailQueueWorker> _logger;
        private readonly IServiceProvider _services;
        private readonly IMessageQueue _queue;

        public EmailQueueWorker(
            ILogger<EmailQueueWorker> logger,
            IServiceProvider services,
            IMessageQueue queue)
        {
            _logger = logger;
            _services = services;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _queue.DequeueAsync(stoppingToken);

                using var scope = _services.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    await emailService.SendAsync(message);
                    _logger.LogInformation("Email sent to {Recipient}", message.To);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {Recipient}", message.To);
                }
            }
        }
    }
}
