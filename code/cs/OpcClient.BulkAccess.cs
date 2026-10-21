// DOC
var commands = new OpcReadNode[5000];
 
for (int index = 0; index < commands.Length; index++)
    commands[index] = new OpcReadNode("ns=2;s=Sensors/Sensor" + (index + 1));
 
IEnumerable<OpcValue> values = client.ReadNodes(commands);


// DOC
var commands = new OpcWriteNode[5000];
 
for (int index = 0; index < commands.Length; index++)
    commands[index] = new OpcWriteNode("ns=2;s=Setpoints/Setpoint" + (index + 1), value: 0.0);
 
OpcStatusCollection results = client.WriteNodes(commands);
 
for (int index = 0; index < results.Count; index++) {
    if (results[index].IsBad)
        Console.WriteLine("{0}: {1}", commands[index].NodeId, results[index].Description);
}



// DOC
client.Services.ReadNodes.MaxNodesPerRead = 500;
client.Services.WriteNodes.MaxNodesPerWrite = 500;
client.Services.Browse.MaxNodesPerBrowse = 100;

// DOC
uint? nodesPerRead = client.Services.ReadNodes.UsedMaxNodesPerRead;

// DOC
var commands = new OpcSubscribeDataChange[1000];
 
for (int index = 0; index < commands.Length; index++) {
    commands[index] = new OpcSubscribeDataChange(
            "ns=2;s=Sensors/Sensor" + (index + 1),
            HandleDataChanged);
}
 
OpcSubscription subscription = client.SubscribeNodes(commands);
...
private static void HandleDataChanged(
        object sender,
        OpcDataChangeReceivedEventArgs e)
{
    OpcMonitoredItem item = (OpcMonitoredItem)sender;
    Console.WriteLine("{0}: {1}", item.NodeId, e.Item.Value);
}
