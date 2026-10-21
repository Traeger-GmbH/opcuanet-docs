'DOC
For Each ns As OpcNamespace In client.Namespaces
    Console.WriteLine("{0}: {1}", ns.Index, ns.Value)
Next

Dim objects As OpcNodeInfo = client.BrowseNode(OpcObjectTypes.ObjectsFolder)

For Each node As OpcNodeInfo In objects.Children()
    Console.WriteLine("{0} ({1})", node.DisplayName.Value, node.NodeId)
Next

'DOC
Dim da As OpcNamespace = client.Namespaces.First(Function(ns) ns.Value.EndsWith("/DA"))
Dim tag = New OpcNodeId("0:Channel1.Device1.Tag1", da.Index)

Dim value As OpcValue = client.ReadNode(tag)
Dim result As OpcStatus = client.WriteNode(tag, 42)

Dim subscription As OpcSubscription = client.SubscribeDataChange(tag, AddressOf HandleDataChanged)

'DOC
client.Interfaces.DataAccess.Organization = OpcClassicOrganization.Flat
client.Interfaces.DataAccess.SupportsBrowseTo = False

client.Connect()

'DOC
Dim [end] As DateTime = DateTime.UtcNow
Dim start As DateTime = [end].AddHours(-1)

Dim raw = client.ReadNodeHistory(start, [end], hdaItemId)
Dim average = client.ReadNodeHistoryProcessed(start, [end], OpcAggregateType.Average, hdaItemId)

For Each historyValue In raw
    Console.WriteLine("{0}: {1}", historyValue.Timestamp, historyValue.Value)
Next

'DOC
Dim subscription As OpcSubscription = client.SubscribeEvent( _
        OpcObjectTypes.Server, _
        AddressOf HandleEventReceived)

subscription.RefreshConditions()
...
Private Sub HandleEventReceived(ByVal sender As Object, ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcAcknowledgeableCondition)

    If condition IsNot Nothing AndAlso Not condition.IsAcked Then
        condition.Acknowledge(client, "Acknowledged by operator")
    End If
End Sub

'DOC
Dim client = New OpcClient("opc.com://remote-host/{E4EBF7FA-CCAC-4125-A611-EAC4981C00EA}")

client.Security.UserIdentity = New OpcClientIdentity("opcuser", "secret")
client.Security.AuthenticationLevel = OpcClassicAuthenticationLevel.PacketIntegrity
client.Security.ImpersonationLevel = OpcClassicImpersonationLevel.Impersonate

client.Connect()

'DOC
Dim identity = New OpcClassicIdentity("opcuser", "secret")

Using discovery = New OpcClassicDiscoveryClient("remote-host", identity)
    For Each endpoint As OpcClassicEndpointDescription In discovery.DiscoverEndpoints()
        Console.WriteLine("{0}: {1}", endpoint.Server.Name, endpoint.Server.Uri)

        For Each serverInterface As OpcClassicInterface In endpoint.Interfaces
            Console.WriteLine("  {0}", serverInterface)
        Next
    Next
End Using

'DOC
Using discovery = New OpcClassicDiscoveryClient("remote-host", identity)
    Dim servers = discovery.DiscoverServers(OpcClassicInterfaces.Da30)
End Using

'DOC
Dim client = New OpcClient(endpoint.Server.Uri)

For Each serverInterface As OpcClassicInterface In endpoint.Interfaces
    client.ServerInterfaces.Add(serverInterface)
Next

client.Connect()

'DOC
client.Interfaces.UseBreakMonitoring = True
AddHandler client.StateChanged, AddressOf HandleStateChanged

client.Connect()
...
Private Sub HandleStateChanged(ByVal sender As Object, ByVal e As OpcClientStateChangedEventArgs)
    Console.WriteLine("{0} -> {1}", e.OldState, e.NewState)
End Sub
