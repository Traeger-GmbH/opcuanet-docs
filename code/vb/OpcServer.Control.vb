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


'DOC
AddHandler server.RequestValidating, AddressOf HandleRequestValidating
...
Private Shared ReadOnly nodesPerSession As New Dictionary(Of OpcNodeId, Integer)()

Private Shared Sub HandleRequestValidating( _
        ByVal sender As Object, _
        ByVal e As OpcRequestValidatingEventArgs)
    Console.Write(" -> Validating: " & e.Request.ToString())

    If e.RequestType = OpcRequestType.AddNodes Then
        Dim sessionId = e.Context.SessionId
        Dim request = CType(e.Request, OpcAddNodesRequest)

        SyncLock nodesPerSession
            Dim count As Integer
            nodesPerSession.TryGetValue(sessionId, count)
            count += request.Commands.Count

            'Reject the request in case it would exceed 100 AddNode commands per session.
            If count > 100 Then
                e.Cancel = True
            Else
                nodesPerSession(sessionId) = count
            End If
        End SyncLock
    End If
End Sub


'DOC
AddHandler server.RequestJudgement, AddressOf HandleRequestJudgement
...
Private Shared Sub HandleRequestJudgement( _
        ByVal sender As Object, _
        ByVal e As OpcRequestJudgementEventArgs)
    'Accept outdated requests, e.g. in case the clocks of client and server differ.
    If e.Impeachment = OpcRequestImpeachment.Outdated Then
        e.Dismiss = True
    End If
End Sub

'DOC
AddHandler server.SessionCreated, Sub(sender, e) Console.WriteLine("Session created: {0}", e.Session.Name)
AddHandler server.SessionClosing, Sub(sender, e) Console.WriteLine("Session closing: {0}", e.Session.Name)

AddHandler server.SubscriptionCreated, Sub(sender, e) Console.WriteLine( _
        "Subscription {0} created in session {1}", _
        e.Subscription.Id, _
        e.Subscription.Session.Name)

'DOC
'Announce the expected end of the maintenance to the clients.
server.EstimatedReturnTime = DateTime.UtcNow.AddMinutes(10)
server.Suspend()
...
server.Resume()
