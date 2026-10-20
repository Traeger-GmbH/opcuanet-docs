Using server = New OpcServer()
    server.Start()

    Using client = New OpcClient(server.Address)
        client.Connect()
        ...
    End Using
End Using
