using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.TestEvents
{
    public class UnreliablyHandledTestEventHandlerGood : IEventHandler<UnreliablyHandledTestEvent>
    {
        public async Task WhenAsync(UnreliablyHandledTestEvent events)
        {
            await Task.CompletedTask;
        }
    }
}
