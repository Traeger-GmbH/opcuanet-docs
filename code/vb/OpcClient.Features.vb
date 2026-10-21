'DOC
Dim speedId As OpcNodeId = client.TranslatePath("/Objects/2:Machine/2:Speed")

If speedId Is Nothing Then
    Console.WriteLine("The path does not exist.")
Else
    Console.WriteLine("Speed: {0}", client.ReadNode(speedId))
End If

'DOC
Dim temperatureId As OpcNodeId = client.TranslatePath( _
        OpcObjectTypes.ObjectsFolder, _
        "2:Machine", _
        "2:Temperature")

'DOC
Dim results As OpcTranslatePathResultCollection = client.TranslatePaths( _
        New OpcTranslatePath("/Objects/2:Machine/2:Speed"), _
        New OpcTranslatePath("/Objects/2:Machine/2:Temperature"), _
        New OpcTranslatePath("/Objects/2:Machine/2:IsRunning"))

For Each result As OpcTranslatePathResult In results
    If result.IsGood Then
        Console.WriteLine(result.NodeId)
    Else
        Console.WriteLine("Not found: {0}", result.Code)
    End If
Next

'DOC
Dim speed As OpcValue = client.ReadNode("nsu=http://example.com/machine/;s=Machine/Speed")

'DOC
Dim index As Integer = client.Namespaces.ResolveIndex("http://example.com/machine/")

If index >= 0 Then
    Dim temperatureId = New OpcNodeId("Machine/Temperature", namespaceIndex:=index)
    Dim temperature As OpcValue = client.ReadNode(temperatureId)
End If

'DOC
Dim registeredNodeIds As OpcNodeIdCollection = client.RegisterNodes( _
        "ns=2;s=Machine/Speed", _
        "ns=2;s=Machine/Temperature")

Dim commands = New OpcReadNode(registeredNodeIds.Count - 1) {}

For index As Integer = 0 To commands.Length - 1
    commands(index) = New OpcReadNode(registeredNodeIds(index))
Next

For cycle As Integer = 0 To 9
    client.ReadNodes(commands)
Next

client.UnregisterNodes(registeredNodeIds)

'DOC
Using discovery = New OpcDiscoveryClient("opc.tcp://localhost:4840/")
    For Each server As OpcApplicationDescription In discovery.DiscoverServers()
        Console.WriteLine("{0} ({1})", server.Name, server.Type)

        For Each url As Uri In server.DiscoveryUrls
            Console.WriteLine("  {0}", url)
        Next
    Next
End Using

'DOC
Dim machine As OpcNodeInfo = client.BrowseNode("ns=2;s=Machine")

Dim firstAlarm As OpcNodeInfo = Descendants(machine) _
        .Where(Function(node) node.Category = OpcNodeCategory.Variable) _
        .FirstOrDefault(Function(node) node.Name.Value.StartsWith("Alarm"))
...
Private Shared Iterator Function Descendants( _
        ByVal node As OpcNodeInfo) As IEnumerable(Of OpcNodeInfo)
    For Each child As OpcNodeInfo In node.Children()
        Yield child

        For Each descendant As OpcNodeInfo In Descendants(child)
            Yield descendant
        Next
    Next
End Function

'DOC
Dim speed = New OpcNodeValue(Of Double)("ns=2;s=Machine/Speed")

Dim current As Double = speed.Read(client)

If speed.Status.IsGood Then
    Dim result As OpcStatus = speed.Write(client, current + 10)
End If

'DOC
Dim temperatures = New OpcNodeValue(Of Double)() { _
    New OpcNodeValue(Of Double)("ns=2;s=Machine/Temperature1"), _
    New OpcNodeValue(Of Double)("ns=2;s=Machine/Temperature2"), _
    New OpcNodeValue(Of Double)("ns=2;s=Machine/Temperature3") _
}

Dim values As Double() = temperatures.Read(client)

'DOC
Using source = New OpcNodeStream(client, "ns=2;s=Machine/Snapshot")
    Using target = IO.File.Create("snapshot.bin")
        source.CopyTo(target)
    End Using
End Using

'DOC
Dim grid = New Double(1, 2) {{1, 2, 3}, {4, 5, 6}}
client.WriteNode("ns=2;s=Machine/Grid", grid)

Dim values As Double(,) = CType(client.ReadNode("ns=2;s=Machine/Grid").Value, Double(,))
Dim value As Double = values(1, 2)

'DOC
Dim nodeIds = New OpcNodeId() {"ns=2;s=Machine/Speed", "ns=2;s=Machine/Temperature"}

Using history = client.ReadNodesHistory( _
        DateTime.UtcNow.AddHours(-1), _
        DateTime.UtcNow, _
        numberOfValues:=100, _
        nodeIds)
    For Each value As OpcHistoryValue In history(nodeIds(0))
        Console.WriteLine("{0}: {1}", value.Timestamp, value.Value)
    Next
End Using

'DOC
Dim history As IDictionary(Of OpcNodeId, IEnumerable(Of OpcHistoryValue)) = _
        client.ReadNodesHistory( _
                DateTime.UtcNow.AddHours(-1), _
                DateTime.UtcNow, _
                "ns=2;s=Machine/Speed", _
                "ns=2;s=Machine/Temperature")

For Each entry In history
    Console.WriteLine("{0}: {1} values", entry.Key, entry.Value.Count())
Next

'DOC
client.Locales.Add("de-DE")
client.Locales.Add("en-US")

client.Connect()

Dim displayName As OpcValue = client.ReadNode("ns=2;s=Machine", OpcAttribute.DisplayName)
