using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IEventHandler<T> : IEventHandlerBase
        where T : EventBase
    {
        Task WhenAsync(T events);
    }

    public interface IEventHandlerBase
    { }
}
