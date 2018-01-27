What is this?
--------------------

EventHubs does not have an Emulator you can run locally, so I'm creating 
a method to use a single queue/function locally for event processing with
a QueueTrigger and an EventHubs function for the remote (Azure) version.

You could set up a development EventHubs instance in Azure, but it far
less flexible then a local emulation environment (line latency, version
differences as you switch branches and have mixed events in the stream
still, setting up and clearing the environment, hacking on something
when the power is out, etc).

Intent
-------------

A web application will be able to publish to either Queue (local) or 
EventHubs (cloud). A corresponding trigger will pick up the event and
process it through some set of handlers that are wired to handle that 
type of event.

Locally, for test purposes, there is also a HttpTrigger to publish
test events onto the queue. This will be replaced by a sample web 
application (potentially a different iteration).

How to run
-------------

Dependencies:

* [.Net Core 2 Install](https://www.microsoft.com/net/learn/get-started/windows)
* [Azure Functions 2-beta](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)
* [Azure Storage Emulator: 5.3 works, 5.1 doesn't](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)

From command-line: `func start`

From command-line 2: 

* curl: `curl --request POST --data " " http://localhost:7071/api/PublishSampleQueueEvents`
* PoSH: `Invoke-WebRequest -Method POST http://localhost:7071/api/PublishSampleQueueEvents`

Design Thinking
------------------

The Async Web App manages it's own data, but publishes events that it's backend or other systems may need to know.

The Common Language (basic dispatch logic, contracts, and global list of events) lives in 
the Common.dll assembly. Consumers can each have their own DLL full of handlers specific
to their business domain.

Example:

* Inventory System
    * When inventory is received, publishes InventoryAdjustment
    * When an OrderPlaced event is received, it attempts to fill it and publishes OrderFilled
* Website
    * When a user places an order, publishes OrderPlaced
    * When OrderFilled is received, updates DB
    * When OrderFilled is received, send email confirmation to user

```

      +--------------------+
      |      User          |
      +--------------------+
           ^           |
           | Browse    | Place Order
           |           |
           |           |
           |           v
      +-----------------------------------------------+
      |                                               |
      |               Store Website + DB              |
      |                                               |
      +-----------------------------------------------+
         ^             |                   ^
         | Update      | OrderPlaced       | Update DB,
         |   DB        |                   | Send Email
         |             v                   |
      +----------------------------------------------->
        O   Event Log  O                  O
      +----------------------------------------------->
        ^                     |           ^
        | InventoryAdjusted   | Fill it?  | OrderFilled
        |                     v           |
      +-----------------------------------------------+
      |                                               |
      |               Inventory System                |
      |                                               |
      +-----------------------------------------------+
```

Now it's easy to add in logic for things like partial order 
fulfillment, split shipments, and accounting system with
associated events, and so on. And they can reboot every 5 minutes
without really affecting each other.
