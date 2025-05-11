
using Messaging.Core.Contracts;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Services
{
    public class RabbitMqMessageQueue : IMessageQueue, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;

        public RabbitMqMessageQueue(string hostName, string queueName)
        {
            _queueName = queueName;
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                DispatchConsumersAsync = true // Habilita consumidores assíncronos
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declaração de fila correta para versões mais recentes
            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task EnqueueAsync(EmailMessage message)
        {
            var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: _queueName,
                basicProperties: properties,
                body: body);

            await Task.CompletedTask;
        }

        public async Task<EmailMessage> DequeueAsync(CancellationToken cancellationToken)
        {
            // Implementação melhorada com consumo assíncrono
            var consumer = new AsyncEventingBasicConsumer(_channel);
            var tcs = new TaskCompletionSource<EmailMessage>();

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var message = System.Text.Json.JsonSerializer.Deserialize<EmailMessage>(ea.Body.Span);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    tcs.TrySetResult(message);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
                return Task.CompletedTask;
            };

            string consumerTag = _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                try
                {
                    return await tcs.Task;
                }
                finally
                {
                    _channel.BasicCancel(consumerTag);
                }
            }
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            GC.SuppressFinalize(this);
        }
    }
}