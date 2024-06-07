using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqAspNetCore.Shared.Options;

namespace RabbitMqAspNetCore.Shared.Services;

public class RabbitMqService
{
    public IConnection Connection { get; private set; }
    public IModel Channel { get; private set; }

    public RabbitMqService(IOptions<RabbitMqOptions> options)
    {
        var rabbitMqOptions = options.Value;
        
        var factory = new ConnectionFactory
        {
            HostName = rabbitMqOptions.Host,
            VirtualHost = rabbitMqOptions.VHost,
            UserName = rabbitMqOptions.Username,
            Password = rabbitMqOptions.Password,
            DispatchConsumersAsync = true
        };
    
        Connection = factory.CreateConnection();
        Channel = Connection.CreateModel();
        
        Channel.QueueDeclare(
            rabbitMqOptions.Queues.TestQueue,
            durable: true,
            exclusive: false,
            autoDelete: false);
        
        Channel.QueueDeclare(
            rabbitMqOptions.Queues.AnotherTestQueue,
            durable: true,
            exclusive: false,
            autoDelete: false);
    }

    public void Send(object obj, string queue, Action<IBasicProperties> configureProperties = null)
    {
        var message = JsonSerializer.Serialize(obj);
        Send(message, queue, configureProperties);
    }
    
    public void Send(string message, string queue, Action<IBasicProperties> configureProperties = null)
    {
        var body = Encoding.UTF8.GetBytes(message);

        var properties = Channel.CreateBasicProperties();
        configureProperties?.Invoke(properties);

        Channel.BasicPublish(
            exchange: "",
            routingKey: queue,
            basicProperties: properties,
            body);
    }
}