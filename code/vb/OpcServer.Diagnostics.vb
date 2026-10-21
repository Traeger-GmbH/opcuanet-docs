' DOC
Dim diagnostics As OpcServerDiagnostics = server.Diagnostics

Console.WriteLine($"Sessions: {diagnostics.CurrentSessionCount} (total {diagnostics.CumulatedSessionCount})")
Console.WriteLine($"Subscriptions: {diagnostics.CurrentSubscriptionCount}")
Console.WriteLine($"Rejected sessions: {diagnostics.RejectedSessionCount}, by security: {diagnostics.SecurityRejectedSessionCount}")
Console.WriteLine($"Rejected requests: {diagnostics.RejectedRequestsCount}, by security: {diagnostics.SecurityRejectedRequestsCount}")
Console.WriteLine($"Session timeouts: {diagnostics.SessionTimeoutCount}, aborts: {diagnostics.SessionAbortCount}")

' DOC
For Each session As OpcSession In server.GetSessions()
    Console.WriteLine($"{session.Name} {session.EndpointUrl} {session.UsedIdentity} since {session.ConnectTime:u}")

    Dim sessionDiagnostics As OpcSessionDiagnostics = session.Diagnostics

    If sessionDiagnostics IsNot Nothing Then
        Console.WriteLine($"  requests: {sessionDiagnostics.TotalRequestCounter.TotalCalls}, failed: {sessionDiagnostics.TotalRequestCounter.FailedCalls}")
        Console.WriteLine($"  unauthorized: {sessionDiagnostics.UnauthorizedRequestsCount}, writes: {sessionDiagnostics.WriteCounter.TotalCalls}")
    End If

    For Each subscription As OpcSubscription In session.GetSubscriptions()
        Dim subscriptionDiagnostics As OpcSubscriptionDiagnostics = subscription.Diagnostics

        If subscriptionDiagnostics IsNot Nothing Then
            Console.WriteLine($"  subscription {subscription.Id}: queue overflows {subscriptionDiagnostics.MonitoringQueueOverflowCount}")
        End If
    Next
Next
