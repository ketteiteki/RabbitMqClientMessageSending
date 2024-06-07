
namespace RabbitMqAspNetCore.Shared.Options;

public class RabbitMqOptions
{
    public static string Name = "RabbitMq";
    
    public string Host { get; set; }

    public string VHost { get; set; }

    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public Queues Queues { get; set; } 
}

public class Queues
{
    public string TestQueue { get; set; }

    public string AnotherTestQueue { get; set; }
}