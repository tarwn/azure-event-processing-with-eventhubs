using Common.Dispatch;
using Common.Tests.TestEvents;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.Dispatch
{
    [TestFixture]
    public class DispatchTests
    {

        [Test]
        public void GetHandlersForRegistration_NoAssemblies_RegistersNoHandlers()
        {
            var assemblyList = new Assembly[] { };

            var handlers = EventDispatcher.GetHandlersForRegistration(assemblyList);

            Assert.AreEqual(0, handlers.Keys.Count);
        }

        [Test]
        public void GetHandlersForRegistration_SingleHandlerClass_IsRegisteredOnce()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };

            var handlers = EventDispatcher.GetHandlersForRegistration(assemblyList);

            Assert.IsNotNull(handlers[typeof(SingleHandledTestEvent)]);
            Assert.AreEqual(1, handlers[typeof(SingleHandledTestEvent)].Count);
        }

        [Test]
        public void GetHandlersForRegistration_MultiHandlerClass_IsRegisteredOnceForEachHandler()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };

            var handlers = EventDispatcher.GetHandlersForRegistration(assemblyList);

            Assert.IsNotNull(handlers[typeof(MultiHandledTestEvent)]);
            Assert.AreEqual(3, handlers[typeof(MultiHandledTestEvent)].Count);
        }

        [Test]
        public void GetHandlersForRegistration_ClassWithMultipleHandlers_IsRegisteredForEachHandledType()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };

            var handlers = EventDispatcher.GetHandlersForRegistration(assemblyList);

            Assert.IsNotNull(handlers[typeof(MultipleHandlerTestEventOne)]);
            Assert.AreEqual("Common.Tests.TestEvents.MultipleHandler", handlers[typeof(MultipleHandlerTestEventOne)][0].ClassName);
            Assert.IsNotNull(handlers[typeof(MultipleHandlerTestEventTwo)]);
            Assert.AreEqual("Common.Tests.TestEvents.MultipleHandler", handlers[typeof(MultipleHandlerTestEventTwo)][0].ClassName);
            Assert.IsNotNull(handlers[typeof(MultipleHandlerTestEventThree)]);
            Assert.AreEqual("Common.Tests.TestEvents.MultipleHandler", handlers[typeof(MultipleHandlerTestEventThree)][0].ClassName);
        }

        [Test]
        public async Task DispatchAsync_EventWithoutHandlers_CompletesWithoutErrors()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };
            var evt = new UnhandledTestEvent();
            var dispatcher = new EventDispatcher(assemblyList);
            var loggerMock = new Mock<IEventDispatcherLogger>();
            
            await dispatcher.DispatchAsync(evt, loggerMock.Object);

            // no exceptions have occurred
            loggerMock.Verify(l => l.LogDispatchAsync(It.IsAny<double>(), evt.GetType(), 0));
        }


        [Test]
        public async Task DispatchAsync_EventWithOneSuccessfulHandler_CompletesWithoutErrors()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };
            var evt = new SingleHandledTestEvent();
            var dispatcher = new EventDispatcher(assemblyList);
            var loggerMock = new Mock<IEventDispatcherLogger>();

            await dispatcher.DispatchAsync(evt, loggerMock.Object);

            // no exceptions have occurred
            loggerMock.Verify(l => l.LogDispatchAsync(It.IsAny<double>(), evt.GetType(), 1));
        }

        [Test]
        public async Task DispatchAsync_EventWithMultipleSuccessfulHandlers_CompletesWithoutErrors()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };
            var evt = new MultiHandledTestEvent();
            var dispatcher = new EventDispatcher(assemblyList);
            var loggerMock = new Mock<IEventDispatcherLogger>();

            await dispatcher.DispatchAsync(evt, loggerMock.Object);

            // no exceptions have occurred
            loggerMock.Verify(l => l.LogDispatchAsync(It.IsAny<double>(), evt.GetType(), 3));
        }

        [Test]
        public void DispatchAsync_EventWithSomeErroringHandlers_CompletesWithRaisedExceptionAndLogs()
        {
            var assemblyList = new Assembly[] {
                Assembly.GetAssembly(this.GetType())
            };
            var evt = new UnreliablyHandledTestEvent();
            var dispatcher = new EventDispatcher(assemblyList);
            var loggerMock = new Mock<IEventDispatcherLogger>();

            AsyncTestDelegate expr = async () => await dispatcher.DispatchAsync(evt, loggerMock.Object);

            Assert.ThrowsAsync<EventDispatcherException>(expr);
            loggerMock.Verify(l => l.LogDispatchAsync(It.IsAny<double>(), evt.GetType(), 2));
        }
    }
}
