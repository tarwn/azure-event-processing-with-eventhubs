using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// Envelope is used to enforce type name handling for Azure Function
    /// serialization so deserialization will deserialize proper child class
    /// automatically. 
    /// </summary>
    /// <remarks>
    /// Also leaves the door open to adding version properties, so consumers
    /// will have visibility into what version of event they are receiving
    /// </remarks>
    public class Envelope
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
        public EventBase Event { get; set; }
    }
}
 