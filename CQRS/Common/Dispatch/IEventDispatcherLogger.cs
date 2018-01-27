using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dispatch
{
    public interface IEventDispatcherLogger
    {
        Task LogDispatchAsync(double msElapsed, Type eventType, int numberOfHandlers);
    }
}
