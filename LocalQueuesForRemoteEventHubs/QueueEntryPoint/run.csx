#r "Newtonsoft.Json"
#r "../Common.dll"

using System;
using Newtonsoft.Json;
using Common;
using Common.Events;
using Common.Dispatch;

private static Type[] _types = new []{
    typeof(SampleEvent)
};
private static EventDispatcher _dispatcher = EventDispatcher.ForAssembliesFromTypes(_types);

public static async Task Run(Envelope serializedItem, TraceWriter log)
{
    var eventDesc = JsonConvert.SerializeObject(serializedItem);
    log.Info($"C# Queue trigger function processed: [{serializedItem.GetType().Name} '{eventDesc}'] from {serializedItem}");

    await _dispatcher.DispatchAsync(serializedItem.Event, new Logger(log));
}

public class Logger : IEventDispatcherLogger
{
    TraceWriter _log;

    public Logger(TraceWriter log)
    {
        _log = log;
    }

    public async Task LogDispatchAsync(double msElapsed, Type eventType, int numberOfHandlers)
    {
        _log.Info($"Event {eventType.FullName} dispatched in {msElapsed}ms on {numberOfHandlers} handlers");
        await Task.CompletedTask;
    }
}
