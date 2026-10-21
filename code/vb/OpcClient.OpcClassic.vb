'DOC
For Each ns As OpcNamespace In client.Namespaces
    Console.WriteLine("{0}: {1}", ns.Index, ns.Value)
Next

Dim objects As OpcNodeInfo = client.BrowseNode(OpcObjectTypes.ObjectsFolder)

For Each node As OpcNodeInfo In objects.Children()
    Console.WriteLine("{0} ({1})", node.DisplayName.Value, node.NodeId)
Next

'DOC
Dim da As OpcNamespace = Nothing

For Each ns As OpcNamespace In client.Namespaces
    If ns.Value.EndsWith("/DA") Then
        da = ns
    End If
Next

Dim tag = New OpcNodeId("0:Channel1.Device1.Tag1", namespaceIndex:=da.Index)

Dim value As OpcValue = client.ReadNode(tag)
Dim result As OpcStatus = client.WriteNode(tag, value:=42)

Dim subscription As OpcSubscription = client.SubscribeDataChange(tag, AddressOf HandleDataChanged)

'DOC
client.Interfaces.DataAccess.Organization = OpcClassicOrganization.Flat
client.Interfaces.DataAccess.SupportsBrowseTo = False

client.Connect()

'DOC
Dim endTime As DateTime = DateTime.UtcNow
Dim startTime As DateTime = endTime.AddHours(-1)

Dim raw = client.ReadNodeHistory(startTime, endTime, hdaItemId)
Dim average = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Average, _
        hdaItemId)

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
        condition.Acknowledge(client, comment:="Acknowledged by operator")
    End If
End Sub

'DOC
Dim client = New OpcClient("opc.com://remote-host/{E4EBF7FA-CCAC-4125-A611-EAC4981C00EA}")

client.Security.UserIdentity = New OpcClientIdentity(userName:="opcuser", password:="secret")
client.Security.AuthenticationLevel = OpcClassicAuthenticationLevel.PacketIntegrity
client.Security.ImpersonationLevel = OpcClassicImpersonationLevel.Impersonate

client.Connect()

'DOC
Dim identity = New OpcClassicIdentity(userName:="opcuser", password:="secret")

Using discovery = New OpcClassicDiscoveryClient(host:="remote-host", identity)
    For Each endpoint As OpcClassicEndpointDescription In discovery.DiscoverEndpoints()
        Console.WriteLine("{0}: {1}", endpoint.Server.Name, endpoint.Server.Uri)

        For Each serverInterface As OpcClassicInterface In endpoint.Interfaces
            Console.WriteLine("  {0}", serverInterface)
        Next
    Next
End Using

'DOC
Using discovery = New OpcClassicDiscoveryClient(host:="remote-host", identity)
    Dim servers As OpcClassicApplicationDescriptionCollection _
            = discovery.DiscoverServers(OpcClassicInterfaces.Da30)
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
Private Shared Sub HandleStateChanged( _
        ByVal sender As Object, _
        ByVal e As OpcClientStateChangedEventArgs)
    Console.WriteLine("{0} -> {1}", e.OldState, e.NewState)
End Sub

