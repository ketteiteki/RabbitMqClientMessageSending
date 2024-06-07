using RabbitMQ.Client.Events;
using RabbitMqAspNetCore.Shared;
using RabbitMqAspNetCore.Shared.Interfaces;

namespace RabbitMqAspNetCore.Receiver.Consumers;

public class AnotherMyConsumer : IConsumer<UserCreatedEvent>, IConsumer<TransactionCreatedEvent>
{
    public async Task Consume(UserCreatedEvent message, AsyncEventingBasicConsumer consumer, BasicDeliverEventArgs args)
    {
        Console.WriteLine($"UserCreatedEvent from AnotherMyConsumer: {message.UserMessage}");
    }

    public async Task Consume(TransactionCreatedEvent message, AsyncEventingBasicConsumer consumer, BasicDeliverEventArgs args)
    {
        Console.WriteLine($"TransactionCreatedEvent from AnotherMyConsumer: {message.TransactionMessage}");
    }
}