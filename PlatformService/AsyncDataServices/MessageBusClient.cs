using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    public MessageBusClient(IConfiguration configuration)
    {
        _configuration = configuration;

        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQhost"],
            Port = int.Parse(_configuration["RabbitMQport"] ?? "0")
        };

        Console.WriteLine($"factory port {factory.Port}");
        Console.WriteLine($"factory host {factory.HostName}");
        Console.WriteLine($"port from config {_configuration["RabbitMQport"] }");


        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            Console.WriteLine($"Connection: {_connection}");
            Console.WriteLine($"_channel: {_channel}");

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            Console.WriteLine($"Cannoted to MessageBus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Cannot establish connection {ex.Message}");
        }

    }
    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("---> RabitMQ connection is open, sending the message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("---> RabitMQ connection is not open, faild to send the message");
            }
    }

    private void Dispose()
    {
        Console.WriteLine("---> Message Disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
        Console.WriteLine("---> New message was sent to RabbitMQ");

    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("RabbitMQ Connection shutdown");
    }
}
