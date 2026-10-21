// DOC
OpcReadNode[] commands = Enumerable.Range(1, 5000)
        .Select(index => new OpcReadNode($"ns=2;s=Sensors/Sensor{index}"))
        .ToArray();

OpcValue[] values = client.ReadNodes(commands).ToArray();

// DOC
OpcWriteNode[] commands = Enumerable.Range(1, 5000)
        .Select(index => new OpcWriteNode($"ns=2;s=Setpoints/Setpoint{index}", 0.0))
        .ToArray();

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
OpcSubscribeDataChange[] commands = Enumerable.Range(1, 1000)
        .Select(index => new OpcSubscribeDataChange(
                $"ns=2;s=Sensors/Sensor{index}",
                HandleDataChanged))
        .ToArray();

OpcSubscription subscription = client.SubscribeNodes(commands);
...
private void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    var item = (OpcMonitoredItem)sender;
    Console.WriteLine("{0}: {1}", item.NodeId, e.Item.Value);
}
