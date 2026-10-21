'DOC
Dim client = New OpcClient("opc.tcp://localhost:4840/")
client.ReconnectTimeout = 5000

Do
    Try
        client.Connect()
        Exit Do
    Catch ex As OpcException
        Console.WriteLine("Connect failed: {0}", ex.Code)
        Thread.Sleep(5000)
    End Try
Loop

'DOC
'Subscribe once after Connect: the client recreates the subscription for a new session.
Dim subscription As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/Speed", _
        AddressOf HandleDataChanged)
...
Private Sub HandleDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    Console.WriteLine("Speed: {0}", e.Item.Value)
End Sub

'DOC
AddHandler client.StateChanged, AddressOf HandleStateChanged
...
Private Sub HandleStateChanged(ByVal sender As Object, ByVal e As OpcClientStateChangedEventArgs)
    Console.WriteLine("{0} -> {1}", e.OldState, e.NewState)

    If e.OldState = OpcClientState.Reconnecting AndAlso e.NewState = OpcClientState.Connected Then
        Console.WriteLine("New session, the subscriptions have been recreated.")
    End If
End Sub

'DOC
AddHandler client.Connected, AddressOf HandleConnected
...
Private nodeIds As OpcNodeId() = {"ns=2;s=Machine/Speed", "ns=2;s=Machine/Temperature"}
Private registeredNodeIds As OpcNodeIdCollection

Private Sub HandleConnected(ByVal sender As Object, ByVal e As EventArgs)
    'Registered node ids are only valid in the session they have been registered in.
    Me.registeredNodeIds = CType(sender, OpcClient).RegisterNodes(Me.nodeIds)
End Sub

'DOC
client.ReconnectTimeout = 0
AddHandler client.BreakDetected, AddressOf HandleBreakDetected
...
Private Sub HandleBreakDetected(ByVal sender As Object, ByVal e As EventArgs)
    Dim client = CType(sender, OpcClient)

    'Decide on your own when and how to connect again.
    Task.Run( _
            Sub()
                Thread.Sleep(10000)
                client.Connect()
            End Sub)
End Sub
