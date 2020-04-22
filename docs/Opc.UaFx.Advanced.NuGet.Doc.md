
# Getting Started

The whole client development guides can be found here:

* [Client Development Guide - English](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide)
* [Client Development Guide - German](https://docs.traeger.de/de/software/sdk/opc-ua/net/client.development.guide)

The whole server development guides can be found here:

* [Server Development Guide - English](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide)
* [Server Development Guide - German](https://docs.traeger.de/de/software/sdk/opc-ua/net/server.development.guide)

A snippet to dig into your server:

```csharp
using Opc.UaFx.Server;
...
var machineNode = new OpcObjectNode("Machine");
var messageNode = new OpcDataVariableNode<string>("Message","Hello World!");

using (var server = new OpcServer(
        "opc.tcp://localhost:4840/", machineNode, messageNode)) {
    server.Start();
    Console.ReadLine();

    // Your further code to interact with the clients.
}
```

A snippet to dig into your client:

```csharp
using Opc.UaFx.Client;
...
using (var client = new OpcClient("opc.tcp://localhost:4840/")) {
    client.Connect();
    Console.WriteLine(client.ReadNode("ns=2;s=Message"));

    // Your further code to interact with the server.
}
```

## Let's Define a Node + Read the Node

```csharp
// ----- Server -----

OpcDataVariableNode<bool> isRunningNode = new OpcDataVariableNode<bool>(
        machineNode,
        "IsRunning",
        value: true);

// ----- Client -----

OpcValue isRunning = client.ReadNode("ns=2;s=Machine/IsRunning");
```

More: [Node Management](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide#node-management)
More: [Read Values of Node(s)](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#reading-values)

## Let's Define a Node Tree + Write some Nodes

```csharp
// ----- Server -----

OpcObjectNode jobNode = new OpcObjectNode(
        machineNode,
        "Job",
        new OpcDataVariableNode<bool>("Cancel", false),
        new OpcDataVariableNode<int>("State", -1));

// ----- Client -----

OpcStatusCollection results = client.WriteNodes(
        new OpcWriteNode("ns=2;s=Machine/Job/Cancel", true),
        new OpcWriteNode("ns=2;s=Machine/Job/State", 0));
```

More: [Node Management](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide#node-management)
More: [Write Values of Node(s)](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#writing-values)

## Let's Define + Read a File Node

```csharp
// ----- Server -----

System.IO.File.WriteAllText("Report.txt", "This is my report.");
OpcFileNode reportNode = new OpcFileNode(machineNode, "Report", "Report.txt");


// ----- Client -----

// All at once
string reportText = OpcFile.ReadAllText(client, "ns=2;s=Machine/Report");

// All via a stream
using (var stream = OpcFile.OpenRead(client, "ns=2;s=Machine/Report")) {
    var reader = new StreamReader(stream);

    while (!reader.EndOfStream)
        Console.WriteLine(reader.ReadLine());
}
```

More: [Providing File Nodes](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide#file-nodes)
More: [Working with File Nodes](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#file-nodes)

## Report and Observe some Alarm's and Event's

```csharp
// ----- Client -----

client.SubscribeEvent(OpcObjectTypes.Server, HandleGlobalEvents);
...

private static void HandleGlobalEvents(object sender, OpcEventReceivedEventArgs e)
{
    Console.WriteLine(e.Event.Message);
}


// ----- Server -----

server.ReportEvent(OpcEventSeverity.Low, "Finished JOB-4711");
```

More: [Providing Events](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide#events)
More: [Working with Events](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#events)

## Report and Observe only Alarm's and Event's from interest

```csharp
// ----- Server -----

OpcAlarmConditionNode temperatureAlarmNode = new OpcAlarmConditionNode(
        machineNode,
        "Temperature");

temperatureAlarmNode.Severity = OpcEventSeverity.High;
temperatureAlarmNode.Message = "Overheating use cases!";


// ----- Client -----

var severity = new OpcSimpleAttributeOperand(
        OpcEventTypes.Event,
        "Severity");

var conditionName = new OpcSimpleAttributeOperand(
        OpcEventTypes.Condition,
        "ConditionName");

var filter = OpcFilter.Using(client)
        .FromEvents(OpcEventTypes.AlarmCondition)
        .Where(severity > OpcEventSeverity.Medium & conditionName.Like("Temperature"))
        .Select();

client.SubscribeEvent(OpcObjectTypes.Server, filter, HandleGlobalEvents);


// ----- Server -----
server.ReportEvent(temperatureAlarmNode);
```

More: [Providing Event Nodes](https://docs.traeger.de/en/software/sdk/opc-ua/net/server.development.guide#event-nodes)
More: [Working with Events](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#event-nodes)
