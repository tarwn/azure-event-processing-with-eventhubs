using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.TestEvents
{
    public class UnreliablyHandledTestEventHandlerErrors : IEventHandler<UnreliablyHandledTestEvent>
    {
        public Task WhenAsync(UnreliablyHandledTestEvent events)
        {
            throw new NullReferenceException("Something in this handler is broken!!!");
        }
    }
}
