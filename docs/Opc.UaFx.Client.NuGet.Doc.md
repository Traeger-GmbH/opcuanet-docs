
# Getting Started

The whole client development guides can be found here:

* [Client Development Guide - English](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide)
* [Client Development Guide - German](https://docs.traeger.de/de/software/sdk/opc-ua/net/client.development.guide)

The most essential snippet to dig in:

```csharp
using Opc.UaFx.Client;
...
using (var client = new OpcClient("opc.tcp://localhost:4840/")) {
    client.Connect();
    // Your code to interact with the server.
}
```

## Let's Read a Node

```csharp
OpcValue isRunning = client.ReadNode("ns=2;s=Machine/IsRunning");
```

More: [Read Values of Node(s)](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#reading-values)

## Let's Write a Node

```csharp
OpcStatus result = client.WriteNode("ns=2;s=Machine/Job/Cancel", true);
```

More: [Write Values of Node(s)](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#writing-values)

## Let's Read a File Node

```csharp
// All at once
string reportText = OpcFile.ReadAllText(client, "ns=2;s=Machine/Report");

// All via a stream
using (var stream = OpcFile.OpenRead(client, "ns=2;s=Machine/Report")) {
    var reader = new StreamReader(stream);

    while (!reader.EndOfStream)
        Console.WriteLine(reader.ReadLine());
}
```

More: [Working with File Nodes](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#file-nodes)

## Browse the Node Tree

```csharp
// One node
OpcNodeInfo machineNode = client.BrowseNode("ns=2;s=Machine");

// A child node
OpcNodeInfo jobNode = machineNode.Child("Job");

// Some child nodes
foreach (var childNode in machineNode.Children()) {
    // Your code to operate on each child node.
}

// Some node attributes
OpcAttributeInfo displayName = machineNode.Attribute(OpcAttribute.DisplayName);

foreach (var attribute in machineNode.Attributes()) {
    // Your code to operate on each attribute.
}
```

More: [Browsing Nodes](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#browsing-nodes)

## Observe some Alarm's and Event's

```csharp
client.SubscribeEvent(OpcObjectTypes.Server, HandleGlobalEvents);

...

private static void HandleGlobalEvents(object sender, OpcEventReceivedEventArgs e)
{
    Console.WriteLine(e.Event.Message);
}
```

More: [Working with Events](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#events)

## Observe only Alarm's and Event's from interest

```csharp
var severity = new OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity");
var conditionName = new OpcSimpleAttributeOperand(OpcEventTypes.Condition, "ConditionName");

var filter = OpcFilter.Using(client)
        .FromEvents(OpcEventTypes.AlarmCondition)
        .Where(severity > OpcEventSeverity.Medium & conditionName.Like("Temperature"))
        .Select();

client.SubscribeEvent(OpcObjectTypes.Server, filter, HandleGlobalEvents);
```

More: [Working with Events](https://docs.traeger.de/en/software/sdk/opc-ua/net/client.development.guide#events)
