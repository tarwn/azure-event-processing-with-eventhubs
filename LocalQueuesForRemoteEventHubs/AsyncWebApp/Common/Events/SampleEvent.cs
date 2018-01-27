using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Events
{
    public class SampleEvent: EventBase
    {
        [Obsolete("Serialization Only", false)]
        public SampleEvent() { }

        public SampleEvent(string name)
            : base()
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
