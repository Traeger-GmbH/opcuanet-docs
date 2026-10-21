'DOC
Dim commands As OpcReadNode() = Enumerable.Range(1, 5000) _
        .Select(Function(index) New OpcReadNode($"ns=2;s=Sensors/Sensor{index}")) _
        .ToArray()

Dim values As OpcValue() = client.ReadNodes(commands).ToArray()

'DOC
Dim commands As OpcWriteNode() = Enumerable.Range(1, 5000) _
        .Select(Function(index) New OpcWriteNode($"ns=2;s=Setpoints/Setpoint{index}", 0.0)) _
        .ToArray()

Dim results As OpcStatusCollection = client.WriteNodes(commands)

For index As Integer = 0 To results.Count - 1
    If results(index).IsBad Then
        Console.WriteLine("{0}: {1}", commands(index).NodeId, results(index).Description)
    End If
Next

'DOC
client.Services.ReadNodes.MaxNodesPerRead = 500
client.Services.WriteNodes.MaxNodesPerWrite = 500
client.Services.Browse.MaxNodesPerBrowse = 100

'DOC
Dim nodesPerRead As UInteger? = client.Services.ReadNodes.UsedMaxNodesPerRead

'DOC
Dim commands As OpcSubscribeDataChange() = Enumerable.Range(1, 1000) _
        .Select(Function(index) New OpcSubscribeDataChange( _
                $"ns=2;s=Sensors/Sensor{index}", _
                AddressOf HandleDataChanged)) _
        .ToArray()

Dim subscription As OpcSubscription = client.SubscribeNodes(commands)
...
Private Sub HandleDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    Dim item = CType(sender, OpcMonitoredItem)
    Console.WriteLine("{0}: {1}", item.NodeId, e.Item.Value)
End Sub
