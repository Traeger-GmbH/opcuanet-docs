'DOC
client.UseBreakDetection = True     'Default: True
client.KeepAlive.Interval = 2000    'Default: 5000
client.ReconnectTimeout = 5000      'Default: 10000

'DOC
AddHandler client.Reconnecting, AddressOf HandleReconnecting
AddHandler client.Reconnected, AddressOf HandleReconnected
AddHandler client.BreakDetected, AddressOf HandleBreakDetected
...
Private Sub HandleReconnecting(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection lost, trying to reconnect.")
End Sub

Private Sub HandleReconnected(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection restored using the existing session.")
End Sub

Private Sub HandleBreakDetected(ByVal sender As Object, ByVal e As EventArgs)
    Console.WriteLine("Connection lost, no automatic reconnect.")
End Sub

'DOC
AddHandler client.KeepAlive.Updated, AddressOf HandleKeepAliveUpdated
...
Private Sub HandleKeepAliveUpdated(ByVal sender As Object, ByVal e As EventArgs)
    Dim keepAlive = CType(sender, OpcClientKeepAlive)

    If keepAlive.IsActive Then
        Console.WriteLine("Server state: {0}, server time: {1}", keepAlive.ServerState, keepAlive.Time)
    Else
        Console.WriteLine("Keep alive failed: {0}", keepAlive.Status)
    End If
End Sub
