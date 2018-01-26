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