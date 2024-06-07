using Microsoft.AspNetCore.Mvc;
using RabbitMqAspNetCore.Shared;
using RabbitMqAspNetCore.Shared.Services;

namespace RabbitMqAspNetCore.Sender.Controllers;

[Route("api")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly RabbitMqService _rabbitMqService;

    public RootController(RabbitMqService rabbitMqService)
    {
        _rabbitMqService = rabbitMqService;
    }

    [HttpPost("transactionCreatedEventSendToTestQueue")]
    public async Task<IActionResult> SendTransactionCreatedEventToTestQueue([FromBody] TransactionCreatedEvent transactionCreatedEvent)
    {
        _rabbitMqService.Send(transactionCreatedEvent, "test-queue", c =>
        {
            c.Headers = new Dictionary<string, object> { {"RequestSource", "UI"} };
        });
        
        return Ok();
    }
    
    [HttpPost("userCreatedEventSendToTestQueue")]
    public async Task<IActionResult> SendUserCreatedEventToTestQueue([FromBody] UserCreatedEvent userCreatedEventModel)
    {
        _rabbitMqService.Send(userCreatedEventModel, "test-queue");
        
        return Ok();
    }
    
    [HttpPost("transactionCreatedEventSendToAnotherTestQueue")]
    public async Task<IActionResult> SendTransactionCreatedEventToAnotherTestQueue([FromBody] TransactionCreatedEvent transactionCreatedEvent)
    {
        _rabbitMqService.Send(transactionCreatedEvent, "another-test-queue", c =>
        {
            c.Headers = new Dictionary<string, object> { {"RequestSource", "UI"} };
        });
        
        return Ok();
    }
    
    [HttpPost("userCreatedEventSendToAnotherTestQueue")]
    public async Task<IActionResult> SendUserCreatedEventToAnotherTestQueue([FromBody] UserCreatedEvent userCreatedEventModel)
    {
        _rabbitMqService.Send(userCreatedEventModel, "another-test-queue");
        
        return Ok();
    }
}