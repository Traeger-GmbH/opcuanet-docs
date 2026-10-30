'DOC
client.UseBreakDetection = True     'Default: True
client.KeepAlive.Interval = 2000    'Default: 5000
client.ReconnectInterval = 5000     'Default: 10000
client.MaxReconnectDuration = 0     'Default: 0 (unlimited)

'DOC
AddHandler client.Reconnecting, AddressOf HandleReconnecting
AddHandler client.Reconnected, AddressOf HandleReconnected
AddHandler client.BreakDetected, AddressOf HandleBreakDetected
...
Private Shared Sub HandleReconnecting(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection lost, trying to reconnect.")
End Sub

Private Shared Sub HandleReconnected(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection restored using the existing session.")
End Sub

Private Shared Sub HandleBreakDetected(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection lost, no automatic reconnect.")
End Sub




'DOC
AddHandler client.KeepAlive.Updated, AddressOf HandleKeepAliveUpdated
...
Private Shared Sub HandleKeepAliveUpdated(ByVal sender As Object, ByVal e As EventArgs)
    Dim keepAlive As OpcClientKeepAlive = CType(sender, OpcClientKeepAlive)

    If keepAlive.IsActive Then
        Console.WriteLine( _
                "Server state: {0}, server time: {1}", _
                keepAlive.ServerState, _
                keepAlive.Time)
    Else
        Console.WriteLine("Keep alive failed: {0}", keepAlive.Status)
    End If
End Sub


