'DOC
Dim value As OpcValue = client.ReadNode("ns=2;s=Machine/Speed")

If value.Status.IsGood Then
    Console.WriteLine("Speed: {0}", value.Value)
ElseIf value.Status.IsUncertain Then
    Console.WriteLine("Speed (uncertain): {0}", value.Value)
Else
    Console.WriteLine("Read failed: {0} ({1})", value.Status.Code, value.Status.Description)
End If

'DOC
Dim result As OpcStatus = client.WriteNode("ns=2;s=Machine/Setpoint", value:=42.0)

If result.Code = OpcStatusCode.BadUserAccessDenied Then
    Console.WriteLine("The user is not allowed to write the setpoint.")
ElseIf result.IsBad Then
    Console.WriteLine("Write failed: {0}", result.Description)
End If

'DOC
Try
    Dim outputs As Object() = client.CallMethod( _
            "ns=2;s=Machine", _
            "ns=2;s=Machine/StartJob", _
            "Job 1")
Catch ex As OpcException
    Console.WriteLine("Call failed: {0} ({1})", ex.Code, ex.Message)
End Try


'DOC
Try
    client.Connect()
Catch ex As OpcException
    If ex.Code = OpcStatusCode.BadSecurityChecksFailed Then
        Console.WriteLine("No endpoint matches the security settings of the client.")
    Else
        Console.WriteLine("Connect failed: {0}", ex.Code)
    End If
End Try
