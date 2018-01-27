using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.TestEvents
{
    public class MultiHandledTestEventHandler : IEventHandler<MultiHandledTestEvent>
    {
        public async Task WhenAsync(MultiHandledTestEvent events)
        {
            await Task.CompletedTask;
        }
    }

    public class MultiHandledTestEventHandler2 : IEventHandler<MultiHandledTestEvent>
    {
        public async Task WhenAsync(MultiHandledTestEvent events)
        {
            await Task.CompletedTask;
        }
    }

    public class MultiHandledTestEventHandler3 : IEventHandler<MultiHandledTestEvent>
    {
        public async Task WhenAsync(MultiHandledTestEvent events)
        {
            await Task.CompletedTask;
        }
    }
}
