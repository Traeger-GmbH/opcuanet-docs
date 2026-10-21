// DOC
OpcServerDiagnostics diagnostics = server.Diagnostics;

Console.WriteLine($"Sessions: {diagnostics.CurrentSessionCount} (total {diagnostics.CumulatedSessionCount})");
Console.WriteLine($"Subscriptions: {diagnostics.CurrentSubscriptionCount}");
Console.WriteLine($"Rejected sessions: {diagnostics.RejectedSessionCount}, by security: {diagnostics.SecurityRejectedSessionCount}");
Console.WriteLine($"Rejected requests: {diagnostics.RejectedRequestsCount}, by security: {diagnostics.SecurityRejectedRequestsCount}");
Console.WriteLine($"Session timeouts: {diagnostics.SessionTimeoutCount}, aborts: {diagnostics.SessionAbortCount}");

// DOC
foreach (OpcSession session in server.GetSessions()) {
    Console.WriteLine($"{session.Name} {session.EndpointUrl} {session.UsedIdentity} since {session.ConnectTime:u}");

    OpcSessionDiagnostics sessionDiagnostics = session.Diagnostics;

    if (sessionDiagnostics != null) {
        Console.WriteLine($"  requests: {sessionDiagnostics.TotalRequestCounter.TotalCalls}, failed: {sessionDiagnostics.TotalRequestCounter.FailedCalls}");
        Console.WriteLine($"  unauthorized: {sessionDiagnostics.UnauthorizedRequestsCount}, writes: {sessionDiagnostics.WriteCounter.TotalCalls}");
    }

    foreach (OpcSubscription subscription in session.GetSubscriptions()) {
        OpcSubscriptionDiagnostics subscriptionDiagnostics = subscription.Diagnostics;

        if (subscriptionDiagnostics != null)
            Console.WriteLine($"  subscription {subscription.Id}: queue overflows {subscriptionDiagnostics.MonitoringQueueOverflowCount}");
    }
}
