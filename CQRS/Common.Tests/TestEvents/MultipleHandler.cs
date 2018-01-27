using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Tests.TestEvents
{
    public class MultipleHandler : IEventHandler<MultipleHandlerTestEventOne>,
                                   IEventHandler<MultipleHandlerTestEventTwo>,
                                   IEventHandler<MultipleHandlerTestEventThree>
    {
        public async Task WhenAsync(MultipleHandlerTestEventOne events)
        {
            await Task.CompletedTask;
        }

        public async Task WhenAsync(MultipleHandlerTestEventTwo events)
        {
            await Task.CompletedTask;
        }

        public async Task WhenAsync(MultipleHandlerTestEventThree events)
        {
            await Task.CompletedTask;
        }
    }
}
