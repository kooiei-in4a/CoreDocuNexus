using jp.in4a.CoreDocuNexus.Contracts.Greetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace jp.in4a.CoreDocuNexus.Api.Greetings;

public class GreetingApi
{
    private readonly ILogger<GreetingApi> _logger;

    public GreetingApi(ILogger<GreetingApi> logger)
    {
        _logger = logger;
    }

    [Function("GetGreeting")]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "greeting")] HttpRequest req, ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var response = new GreetingResponse // Contracts‚ÌDTO‚ðŽg—p
        {
            Message = $"Hello from the API! ({typeof(GreetingApi).Name}) The time is {DateTime.UtcNow:O}",
            Timestamp = DateTime.UtcNow
        };

        return new OkObjectResult(response);
    }
}