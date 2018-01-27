using Common;
using Common.Events.Inventory;
using System;
using System.Threading.Tasks;

namespace OrderIntegration
{
    public class OrderFilledHandler : IEventHandler<OrderFilledEvent>
    {
        public async Task WhenAsync(OrderFilledEvent events)
        {
            // update the database and email an order status update
            await Task.CompletedTask;
        }
    }
}
