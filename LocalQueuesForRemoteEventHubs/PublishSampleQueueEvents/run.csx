#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static IActionResult Run(HttpRequest req, ICollector<CustomQueueMessage> localEventsQueue, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    localEventsQueue.Add(new CustomQueueMessage{ Name = "Test" });
    return new OkObjectResult("Success!");
}

public class CustomQueueMessage
{
    public string Name{ get; set;}
}