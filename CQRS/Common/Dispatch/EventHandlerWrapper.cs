using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Common.Dispatch
{
    public class EventHandlerWrapper
    {
        private MethodInfo _method;
        private Type _class;

        public EventHandlerWrapper(Type eventType, Type handler)
        {
            _class = handler;
            _method = handler.GetMethod("WhenAsync", new Type[] { eventType });
            EventType = eventType;
            ClassName = _class.FullName;
        }

        public Type EventType { get; private set; }
        public string ClassName { get; set; }

        public async Task ExecuteAsync(EventBase evt)
        {
            var instance = _class.GetConstructor(new Type[] { }).Invoke(new object[] { });
            await (Task) _method.Invoke(instance, new object[] { evt });
        }
    }
}
