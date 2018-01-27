using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Dispatch
{
    public class EventDispatcherException : Exception
    {
        public EventDispatcherException(string eventName, IEnumerable<string> handlerNames, IEnumerable<Exception> exceptions)
            : base($"Event dispatch for {eventName} failed for {exceptions.Count()} handlers: {String.Join(", ", handlerNames)}", exceptions.FirstOrDefault())
        {
            ImpactedHandlers = handlerNames.ToList();
            Exceptions = exceptions.ToList();
        }

        public List<string> ImpactedHandlers { get; }
        public List<Exception> Exceptions { get; }
    }
}
