using MiniApm.Consumer.Models;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using MiniApm.Infrastructure.Persistence;
using MiniApm.Domain.Entities;
using Microsoft.EntityFrameworkCore;


var options = new DbContextOptionsBuilder<MiniApmDbContext>()
    .UseSqlServer("Server=localhost,1433;Database=MiniApmDb;User Id=sa;Password=Sifre123!;TrustServerCertificate=True;")
    .Options;

var dbContext = new MiniApmDbContext(options);

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Exchange
channel.ExchangeDeclare(
    exchange: "apm.metrics",
    type: ExchangeType.Topic,
    durable: true
);

// Queue
var queueName = "apm.metrics.processor";

channel.QueueDeclare(
    queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

// Bind
channel.QueueBind(
    queue: queueName,
    exchange: "apm.metrics",
    routingKey: "metrics.mac"
);

Console.WriteLine("Waiting for messages...");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var metrics = JsonSerializer.Deserialize<SystemMetricsMessage>(message, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });

    if (metrics is null)
    {
        Console.WriteLine("Parse hatası");
        return;
    }

    var entity = new SystemMetric
    {
        MachineName = metrics.MachineName,
        Os = metrics.Os,
        CpuUsage = metrics.CpuUsage,
        CpuCoreCount = metrics.CpuCoreCount,
        MemoryUsage = metrics.MemoryUsage,
        TotalMemory = metrics.TotalMemory,
        AvailableMemory = metrics.AvailableMemory,
        UsedMemory = metrics.UsedMemory,
        DiskUsage = metrics.DiskUsage,
        TotalDisk = metrics.TotalDisk,
        FreeDisk = metrics.FreeDisk,
        UsedDisk = metrics.UsedDisk,
        Timestamp = metrics.Timestamp
    };

    dbContext.SystemMetrics.Add(entity);
    dbContext.SaveChanges();

    Console.WriteLine("Saved to DB");
};

channel.BasicConsume(
    queue: queueName,
    autoAck: true,
    consumer: consumer
);

Console.ReadLine();