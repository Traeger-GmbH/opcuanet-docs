'DOC
Dim value As OpcValue = client.ReadNode("ns=2;s=Machine/Job/Speed")

'DOC
If value.Status.IsGood Then
    'Your code to operate on the value.
End If

'DOC
Dim intValue As Integer = CInt(value.Value)

'DOC
Dim intValues As Integer() = CType(value.Value, Integer())

'DOC
Dim status As OpcStatus = client.WriteNode("ns=2;s=Machine/Job/Speed", 1200)

'DOC
Dim values As Integer() = New Integer(2) {1200, 1350, 1780}
Dim status As OpcStatus = client.WriteNode("ns=2;s=Machine/Job/Speeds", values)

'DOC
If Not status.IsGood Then
    'Your code to handle a failed write operation.
End If

'DOC
Using client As New OpcClient("opc.tcp://localhost:4840")
    client.Connect()
    Dim arrayValue As OpcValue = client.ReadNode("ns=2;s=Machine/Job/Speeds")

    If arrayValue.Status.IsGood Then
        Dim intArrayValue As Integer() = CType(arrayValue.Value, Integer())

        intArrayValue(2) = 100
        intArrayValue(4) = 200
        intArrayValue(9) = 300

        Dim status As OpcStatus = client.WriteNode("ns=2;s=Machine/Job/Speeds", intArrayValue)

        If Not status.IsGood Then _
            Console.WriteLine("Failed to write array value!")
    End If
End Using
