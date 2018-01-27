using Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Handlers
{
    public class SampleHandler : IEventHandler<SampleEvent>
    {
        public async Task WhenAsync(SampleEvent evt)
        {
            Console.WriteLine($"Processing event {evt.GetType().Name} with name '{evt.Name}'");

            await Task.CompletedTask;
        }
    }
}
