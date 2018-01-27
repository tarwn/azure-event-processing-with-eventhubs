using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.TestEvents
{
    public class SingleHandledTestEventHandler : IEventHandler<SingleHandledTestEvent>
    {
        public async Task WhenAsync(SingleHandledTestEvent evt)
        {
            await Task.CompletedTask;
        }
    }
}
