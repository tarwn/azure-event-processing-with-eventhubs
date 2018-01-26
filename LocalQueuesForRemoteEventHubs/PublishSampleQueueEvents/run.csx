#r "Newtonsoft.Json"
#r "../Common.dll"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Common;
using Common.Events;

public static async Task<IActionResult> Run(HttpRequest req, IAsyncCollector<Envelope> localEventsQueue, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    await localEventsQueue.AddAsync(new Envelope {
        Event = new SampleEvent("This is a test")
    });
    return new OkObjectResult("Success!");
}
