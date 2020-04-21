'DOC
Imports Opc.UaFx.Client

'DOC
Dim client = New OpcClient("opc.tcp://localhost:4840/")

'DOC
client.Connect()

'DOC
'Your code to interact with the server.

'DOC
client.Disconnect()

'DOC
Using client As New OpcClient("opc.tcp://localhost:4840/")
    client.Connect()
    'Your code to interact with the server.
End Using
