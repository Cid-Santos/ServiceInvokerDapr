using Messaging.Core.Contracts;
using Messaging.Infrastructure.Data;
using Messaging.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Adicione no Program.cs
builder.Services.AddDbContext<MessageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// No Program.cs da aplicação que usará a fila (API ou Worker)
builder.Services.AddSingleton<IMessageQueue>(provider =>
    new RabbitMqMessageQueue(
        builder.Configuration["RabbitMQ:HostName"],
        builder.Configuration["RabbitMQ:QueueName"]));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
