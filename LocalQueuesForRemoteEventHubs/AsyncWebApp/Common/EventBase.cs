using Newtonsoft.Json;
using System;

namespace Common
{
    public class EventBase
    {
        [Obsolete("For serialization only", false)]
        public EventBase() { }

        public EventBase(string eventType)
        {
            EventType = eventType;
        }

        public string EventType { get; set; }
    }
}
