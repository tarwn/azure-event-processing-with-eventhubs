What is this?
--------------------

EventHubs does not have an Emulator you can run locally, so I'm creating 
a method to use a single queue/function locally for event processing with
a QueueTrigger and an EventHubs function for the remote (Azure) version.

_Funny Story: Using a Queue instead of EventHubs sort of makes sense...
for about 3 seconds, then you start thinking about multiple consumers
and such and realize it totally won't work, update coming..._

You could set up a development EventHubs instance in Azure, but it far
less flexible then a local emulation environment (line latency, version
differences as you switch branches and have mixed events in the stream
still, setting up and clearing the environment, hacking on something
when the power is out, etc).

_Well, darnit. I may be about to write an offline emulator for EventHubs_

Intent
-------------

Integrate multiple systems via event pipelines. Using an event pipeline
decouples the systems which would normally be more tightly coupled with
service calls/schemas. You still need a common language for Events, but
you should be able to power cycle the services every few minutes, test
drive new versions of systems, and all sorts of things with this model.

Azure Functions will be used as the consumers to consume events and
process them through handler logic. Each "system" integration would
consist of a consumer function to process events into the system. For
sake of an easy sample, I'm also going to create an HTTP function for each
to serve as the "system" and publish events into the pipeline.

Still evolving.

```
        (Users)                      (Users)

          + ^    Direct                 ^    Timely
          | |  Interaction!             | Communication!
          v +                           +
   +----------------+           +---------------+
   |  Your System!  |           |  Functions!   |
   |----------------|           |---------------|
   |                |           |               |
   |                |           |               |
   |                | <-------+ |               |
   |                |  Updates  |               |
   |                |           |               |
   +----------------+           +---------------+
           +                           ^
           | Events                    | Events
           |                           |
           v                           +
        ===================================
               Event Pipeline   +----->
        ===================================
```

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
