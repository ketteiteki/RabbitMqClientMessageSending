using RabbitMqAspNetCore.Receiver.BackgroundServices;
using RabbitMqAspNetCore.Receiver.Consumers;
using RabbitMqAspNetCore.Receiver.Extensions;
using RabbitMqAspNetCore.Shared.Options;
using RabbitMqAspNetCore.Shared.Services;

var builder = WebApplication.CreateBuilder();

var rabbitMqOptionsSelection = builder.Configuration.GetSection(RabbitMqOptions.Name);
var rabbitMqOptions = rabbitMqOptionsSelection.Get<RabbitMqOptions>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RabbitMqOptions>(rabbitMqOptionsSelection);

builder.Services.AddSubscriptions(x =>
{
    x.Configure<MyConsumer>(rabbitMqOptions.Queues.TestQueue);
    x.Configure<AnotherMyConsumer>(rabbitMqOptions.Queues.AnotherTestQueue);
});

builder.Services.AddSingleton<RabbitMqService>();

builder.Services.AddHostedService<RabbitMqBackgroundService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.Run();