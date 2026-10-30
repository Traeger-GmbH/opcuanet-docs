'DOC
Dim diagnostics As OpcServerDiagnostics = server.Diagnostics

Console.WriteLine( _
        "Sessions: {0} (total {1})", _
        diagnostics.CurrentSessionCount, _
        diagnostics.CumulatedSessionCount)

Console.WriteLine( _
        "Subscriptions: {0}", _
        diagnostics.CurrentSubscriptionCount)

Console.WriteLine( _
        "Rejected sessions: {0}, by security: {1}", _
        diagnostics.RejectedSessionCount, _
        diagnostics.SecurityRejectedSessionCount)

Console.WriteLine( _
        "Rejected requests: {0}, by security: {1}", _
        diagnostics.RejectedRequestsCount, _
        diagnostics.SecurityRejectedRequestsCount)

Console.WriteLine( _
        "Session timeouts: {0}, aborts: {1}", _
        diagnostics.SessionTimeoutCount, _
        diagnostics.SessionAbortCount)

'DOC
For Each session As OpcSession In server.GetSessions()
    Console.WriteLine( _
            "{0} {1} {2} since {3:u}", _
            session.Name, _
            session.EndpointUrl, _
            session.UsedIdentity, _
            session.ConnectTime)

    Dim sessionDiagnostics As OpcSessionDiagnostics = session.Diagnostics

    If sessionDiagnostics IsNot Nothing Then
        Console.WriteLine( _
                "  requests: {0}, failed: {1}", _
                sessionDiagnostics.TotalRequestCounter.TotalCalls, _
                sessionDiagnostics.TotalRequestCounter.FailedCalls)

        Console.WriteLine( _
                "  unauthorized: {0}, writes: {1}", _
                sessionDiagnostics.UnauthorizedRequestsCount, _
                sessionDiagnostics.WriteCounter.TotalCalls)
    End If

    For Each subscription As OpcSubscription In session.GetSubscriptions()
        Dim subscriptionDiagnostics As OpcSubscriptionDiagnostics = subscription.Diagnostics

        If subscriptionDiagnostics IsNot Nothing Then
            Console.WriteLine( _
                    "  subscription {0}: queue overflows {1}", _
                    subscription.Id, _
                    subscriptionDiagnostics.MonitoringQueueOverflowCount)
        End If
    Next
Next

'DOC
server.Diagnostics.IsEnabled = True
server.Start()
