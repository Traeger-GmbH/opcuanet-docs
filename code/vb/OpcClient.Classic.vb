'DOC
Imports Opc.UaFx.Classic
Imports Opc.UaFx.Client

'DOC
Dim client = As New OpcClient("opc.com://localhost:4840/<progId>/<classId>")

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
