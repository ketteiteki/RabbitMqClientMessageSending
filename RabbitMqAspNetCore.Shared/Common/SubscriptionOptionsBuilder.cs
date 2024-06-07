using Microsoft.Extensions.DependencyInjection;
using RabbitMqAspNetCore.Shared.Interfaces;

namespace RabbitMqAspNetCore.Shared.Common;

public class SubscriptionOptionsBuilder
{
    public readonly Dictionary<string, IConsumer> Dictionary = new();
    private readonly IServiceProvider _serviceProvider;

    public SubscriptionOptionsBuilder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Configure<T>(string queue) where T : IConsumer
    {
        var consumer = ActivatorUtilities.CreateInstance<T>(_serviceProvider);

        if (Dictionary.TryGetValue(queue, out _))
        {
            throw new Exception("Обработчик для данной очереди уже зарегистрирован");
        }
        
        Dictionary.Add(queue, consumer);
    }
}