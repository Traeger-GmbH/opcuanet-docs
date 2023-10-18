' DOC
Private Shared nodesPerSession As New Dictionary(Of OpcNodeId, Integer)()

Private Shared Sub HandleRequestValidating(sender As Object, e As OpcRequestValidatingEventArgs)
    Console.Write(" -> Validating: " & e.Request.ToString())

    If e.RequestType = OpcRequestType.AddNodes Then
        Dim sessionId = e.Context.SessionId
        Dim request As OpcAddNodesRequest = CType(e.Request, OpcAddNodesRequest)

        SyncLock sender
            If Not nodesPerSession.TryGetValue(sessionId, count) Then
                nodesPerSession.Add(sessionId, count)
            End If

            count += request.Commands.Count
            nodesPerSession(sessionId) = count

            e.Cancel = (count >= 100)
        End SyncLock
    End If
End Sub

' ...
AddHandler server.RequestValidating, AddressOf HandleRequestValidating

' DOC
Sub HandleRequestValidated(sender As Object, e As OpcRequestValidatedEventArgs)
    Console.Write(" -> Validated")
End Sub

' ...

AddHandler server.RequestValidated, AddressOf HandleRequestValidated


' DOC
Private Shared Sub HandleRequestProcessed(sender As Object, e As OpcRequestProcessedEventArgs)
    If e.Response.Success Then
        Console.WriteLine(" -> Processed!")
    Else
        Console.WriteLine(" -> FAILED: {0}!", If(e.Exception?.Message, e.Response.ToString()))
    End If
End Sub

' ...

AddHandler server.RequestProcessed, AddressOf HandleRequestProcessed

' DOC
Private Shared Sub HandleRequestProcessing(sender As Object, e As OpcRequestProcessingEventArgs)
    Console.Write("Processing: " & e.Request.ToString())
End Sub

' ...

AddHandler server.RequestProcessing, AddressOf HandleRequestProcessing

