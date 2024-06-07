using RabbitMqAspNetCore.Shared.Interfaces;

namespace RabbitMqAspNetCore.Shared;

public class TransactionCreatedEvent : IEvent
{
    public string TransactionMessage { get; set; }
}