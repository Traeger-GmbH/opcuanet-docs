'DOC
Dim commands = New OpcReadNode(4999) {}

For index As Integer = 0 To commands.Length - 1
    commands(index) = New OpcReadNode("ns=2;s=Sensors/Sensor" & (index + 1))
Next

Dim values As IEnumerable(Of OpcValue) = client.ReadNodes(commands)

'DOC
Dim commands = New OpcWriteNode(4999) {}

For index As Integer = 0 To commands.Length - 1
    commands(index) = New OpcWriteNode("ns=2;s=Setpoints/Setpoint" & (index + 1), value:=0.0)
Next

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
Dim commands = New OpcSubscribeDataChange(999) {}

For index As Integer = 0 To commands.Length - 1
    commands(index) = New OpcSubscribeDataChange( _
            "ns=2;s=Sensors/Sensor" & (index + 1), _
            AddressOf HandleDataChanged)
Next

Dim subscription As OpcSubscription = client.SubscribeNodes(commands)
...
Private Shared Sub HandleDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    Dim item As OpcMonitoredItem = CType(sender, OpcMonitoredItem)
    Console.WriteLine("{0}: {1}", item.NodeId, e.Item.Value)
End Sub

