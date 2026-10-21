'DOC
Dim speedNode = New OpcDataVariableNode(Of Integer)("Speed", 100)

Using server = New OpcServer("opc.tcp://localhost:48400/", speedNode)
    Using client = New OpcClient("opc.tcp://localhost:48400/")
        server.Start()
        client.Connect()

        Dim value As OpcValue = client.ReadNode(speedNode.Id)

        If CInt(value.Value) <> 100 Then
            Throw New Exception("Unexpected speed.")
        End If
    End Using
End Using

'DOC
Dim result As OpcStatus = client.WriteNode(speedNode.Id, 120)

If Not result.IsGood OrElse speedNode.Value <> 120 Then
    Throw New Exception("The value has not been written.")
End If

'DOC
Using received = New ManualResetEventSlim()
    client.SubscribeDataChange( _
            speedNode.Id, _
            Sub(sender, e)
                If e.Item.Value.As(Of Integer)() = 150 Then
                    received.Set()
                End If
            End Sub)

    speedNode.Value = 150
    speedNode.ApplyChanges(server.SystemContext)

    If Not received.Wait(TimeSpan.FromSeconds(5)) Then
        Throw New Exception("No data change received.")
    End If
End Using

'DOC
Dim machine = New OpcObjectNode("Machine")
Dim startJob = New OpcMethodNode(machine, "StartJob", New Func(Of String, Integer)(AddressOf Me.StartJob))

Using server = New OpcServer("opc.tcp://localhost:48401/", machine)
    Using client = New OpcClient("opc.tcp://localhost:48401/")
        server.Start()
        client.Connect()

        Dim outputs As Object() = client.CallMethod(machine.Id, startJob.Id, "Job 1")

        If CInt(outputs(0)) <> 42 Then
            Throw New Exception("Unexpected job number.")
        End If
    End Using
End Using
...
Private Function StartJob(ByVal name As String) As Integer
    Return 42
End Function
