#r "Newtonsoft.Json"
#r "../Common.dll"

using System;
using Newtonsoft.Json;
using Common;
using Common.Events;

public static void Run(Envelope serializedItem, TraceWriter log)
{
    var eventDesc = JsonConvert.SerializeObject(serializedItem);
    log.Info($"C# Queue trigger function processed: [{serializedItem.GetType().Name} '{eventDesc}'] from {serializedItem}");
}

