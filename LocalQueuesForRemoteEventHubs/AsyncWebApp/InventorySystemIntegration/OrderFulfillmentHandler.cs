using Common;
using Common.Events.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemIntegration
{
    public class OrderFulfillmentHandler : IEventHandler<OrderPlacedEvent>
    {
        public async Task WhenAsync(OrderPlacedEvent events)
        {
            // super complex asyncronous logic with the inventory system!
            await Task.CompletedTask;
        }
    }
}
