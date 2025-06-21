using jp.in4a.CoreDocuNexus.Contracts.Greetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace jp.in4a.CoreDocuNexus.Api;

public class Greeting
{
    private readonly ILogger<Greeting> _logger;

    public Greeting(ILogger<Greeting> logger)
    {
        _logger = logger;
    }

    [Function("Greeting")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Hello from the API! ({typeof(Greeting).Name}) The time is {DateTime.UtcNow:O}");

        //_logger.LogInformation("C# HTTP trigger function processed a request.");

        //var response = new GreetingResponse
        //{
        //    Message = $"Hello from the API! ({typeof(GreetingApi).Name}) The time is {DateTime.UtcNow:O}",
        //    Timestamp = DateTime.UtcNow
        //};

        //return new OkObjectResult(response);
    }
}