using RabbitMqAspNetCore.Shared.Interfaces;

namespace RabbitMqAspNetCore.Shared;

public class UserCreatedEvent : IEvent
{
    public string UserMessage { get; set; }
}