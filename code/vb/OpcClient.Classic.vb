'DOC
Imports Opc.UaFx.Client
Imports Opc.UaFx.Client.Classic

'DOC
Dim client As New OpcClient("opc.com://localhost:4840/<progId>/<classId>")

'DOC
client.Connect()

'DOC
'Your code to interact with the server.

'DOC
client.Disconnect()

'DOC
Using client As New OpcClient("opc.com://localhost:4840/<progId>/<classId>")
    client.Connect()
    'Your code to interact with the server.
End Using
