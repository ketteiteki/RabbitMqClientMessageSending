using RabbitMqAspNetCore.Shared.Common;

namespace RabbitMqAspNetCore.Receiver.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddSubscriptions(this IServiceCollection serviceCollection, Action<SubscriptionOptionsBuilder> action)
    {
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var builder = new SubscriptionOptionsBuilder(serviceProvider);
        action(builder);

        serviceCollection.AddSingleton(x => builder.Dictionary);
        
        return serviceCollection;
    }
}