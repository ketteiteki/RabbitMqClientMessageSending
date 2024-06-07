using RabbitMQ.Client.Events;

namespace RabbitMqAspNetCore.Shared.Interfaces;

public interface IConsumer
{
    
}

public interface IConsumer<in T> : IConsumer
{
    Task Consume(T message, AsyncEventingBasicConsumer consumer, BasicDeliverEventArgs args);
}