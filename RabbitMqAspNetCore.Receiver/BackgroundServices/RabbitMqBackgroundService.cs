using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqAspNetCore.Shared.Common;
using RabbitMqAspNetCore.Shared.Interfaces;
using RabbitMqAspNetCore.Shared.Services;

namespace RabbitMqAspNetCore.Receiver.BackgroundServices;

public class RabbitMqBackgroundService : BackgroundService
{
    private readonly RabbitMqService _rabbitMqService;
    private readonly Dictionary<string, IConsumer> _consumerDictionary; 

    public RabbitMqBackgroundService(
        Dictionary<string, IConsumer> consumerDictionary, 
        RabbitMqService rabbitMqService)
    {
        _consumerDictionary = consumerDictionary;
        _rabbitMqService = rabbitMqService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        foreach (var item in _consumerDictionary)
        {
            var consumer = new AsyncEventingBasicConsumer(_rabbitMqService.Channel);
            consumer.Received += async (sender, eventArgs) =>
            {
                var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                var consumer = item.Value;

                await InvokeConsumerMethodByMatchedContent(consumer, content, sender, eventArgs);
                
                _rabbitMqService.Channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            _rabbitMqService.Channel.BasicConsume(item.Key, false, consumer);
        }
    }

    private async Task InvokeConsumerMethodByMatchedContent(IConsumer consumer, string content, object sender, BasicDeliverEventArgs eventArgs)
    {
        var genericTypes = consumer.GetType().GetInterfaces().Where(x => 
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IConsumer<>))
            .Select(x => x.GetGenericArguments().First());

        foreach (var type in genericTypes)
        {
            if (StrictJsonSerializer.TryDeserialize(content, type, out var message))
            {
                var classMethod = consumer.GetType().GetMethods()
                    .FirstOrDefault(m => m.Name == "Consume" &&
                                         m.GetParameters().First().ParameterType == type);
                
                var task = (Task)classMethod.Invoke(consumer, new[] { message, sender, eventArgs });
                
                await task;
            }
        }      
    }
    
    public override void Dispose()
    {
        _rabbitMqService.Connection.Close();
        _rabbitMqService.Channel.Close();
        base.Dispose();
    }
}
