Using discoveryClient As New OpcClassicDiscoveryClient("<host>")
    Dim servers = discoveryClient.DiscoverServers()

    For Each server In servers
        Console.WriteLine( _
                "- {0}, ClassId={1}, ProgId={2}", _
                server.Name, _
                server.ClassId, _
                server.ProgId)
    Next
End Using