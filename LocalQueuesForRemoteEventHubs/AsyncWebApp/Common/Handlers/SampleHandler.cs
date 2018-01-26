using Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Handlers
{
    public class SampleHandler : IEventHandler<SampleEvent>
    {
        public async Task WhenAsync(SampleEvent[] events)
        {
            foreach(var e in events) {
                Console.WriteLine($"Processing event {e.EventType} with name '{e.Name}'");
            }

            await Task.CompletedTask;
        }
    }
}
