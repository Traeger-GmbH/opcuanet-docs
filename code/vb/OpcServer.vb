'DOC
Imports Opc.UaFx.Server

'DOC
Dim server = New OpcServer("opc.tcp://localhost:4840/")

'DOC
server.Start()

'DOC
'Your code to process client requests.

'DOC
server.Stop()

'DOC
Using server As new OpcServer("opc.tcp://localhost:4840/")
    server.Start()
    'Your code to process client requests.
End Using
