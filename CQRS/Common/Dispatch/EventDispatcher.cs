using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dispatch
{
    public class EventDispatcher
    {
        private Dictionary<Type, List<EventHandlerWrapper>> _registeredHandlers;

        public EventDispatcher()
            : this(new [] { Assembly.GetAssembly(typeof(EventDispatcher)) })
        { }

        public EventDispatcher(Assembly[] assemblies)
        {
            _registeredHandlers = GetHandlersForRegistration(assemblies);
        }

        public static EventDispatcher ForAssembliesFromTypes(Type[] types)
        {
            var assemblies = types.Select(t => Assembly.GetAssembly(t))
                                  .Distinct()
                                  .ToArray();
            return new EventDispatcher(assemblies);
        }

        public static Dictionary<Type, List<EventHandlerWrapper>> GetHandlersForRegistration(Assembly[] assemblies)
        {
            var wrappers = assemblies.SelectMany(a => a.GetTypes())
                                     .Where(t => typeof(IEventHandlerBase).IsAssignableFrom(t))
                                     .SelectMany(t => t.GetMethods())
                                     .Where(m => m.Name == "WhenAsync")
                                     .Select(m => new EventHandlerWrapper(m.GetParameters()[0].ParameterType, m.DeclaringType));

            var registeredHandlers = new Dictionary<Type, List<EventHandlerWrapper>>();
            foreach (var wrapper in wrappers)
            {
                if (!registeredHandlers.ContainsKey(wrapper.EventType))
                {
                    registeredHandlers.Add(wrapper.EventType, new List<EventHandlerWrapper>());
                }

                registeredHandlers[wrapper.EventType].Add(wrapper);
            }
            return registeredHandlers;
        }

        public async Task DispatchAsync(EventBase evt, IEventDispatcherLogger logger)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (_registeredHandlers.ContainsKey(evt.GetType()))
            {
                var handlers = _registeredHandlers[evt.GetType()].Select(h => (Task: h.ExecuteAsync(evt), Name: h.ClassName))
                                                                 .ToList(); // force evaluation of Task so it isn't re-run
                try
                {
                    await Task.WhenAll(handlers.Select(h => h.Task));
                }
                catch
                {
                    // Task.WhenAll only reports the first exception, to make it easier for developers
                    //  who found AggregateException difficult and prefer throwing information away
                    //  and put the pain on the developer who has to debug with less information.
                    //
                    // So instead we'll accept reality and harvest the exceptions ourselves
                    var exceptions = handlers.Where(h => h.Task.IsFaulted)
                                             .Select(h => h.Task.Exception);
                    var names = handlers.Where(h => h.Task.IsFaulted)
                                             .Select(h => h.Name);
                    throw new EventDispatcherException(evt.GetType().FullName, names, exceptions);
                }
                finally
                {
                    await logger.LogDispatchAsync(stopwatch.Elapsed.TotalMilliseconds, evt.GetType(), handlers.Count());
                }
            }
            else
            {
                await logger.LogDispatchAsync(stopwatch.Elapsed.TotalMilliseconds, evt.GetType(), 0);
            }
        }

    }
}
