// DOC
OpcServerDiagnostics diagnostics = server.Diagnostics;
 
Console.WriteLine(
        "Sessions: {0} (total {1})",
        diagnostics.CurrentSessionCount,
        diagnostics.CumulatedSessionCount);
 
Console.WriteLine(
        "Subscriptions: {0}",
        diagnostics.CurrentSubscriptionCount);
 
Console.WriteLine(
        "Rejected sessions: {0}, by security: {1}",
        diagnostics.RejectedSessionCount,
        diagnostics.SecurityRejectedSessionCount);
 
Console.WriteLine(
        "Rejected requests: {0}, by security: {1}",
        diagnostics.RejectedRequestsCount,
        diagnostics.SecurityRejectedRequestsCount);
 
Console.WriteLine(
        "Session timeouts: {0}, aborts: {1}",
        diagnostics.SessionTimeoutCount,
        diagnostics.SessionAbortCount);

// DOC
foreach (OpcSession session in server.GetSessions()) {
    Console.WriteLine(
            "{0} {1} {2} since {3:u}",
            session.Name,
            session.EndpointUrl,
            session.UsedIdentity,
            session.ConnectTime);
 
    OpcSessionDiagnostics sessionDiagnostics = session.Diagnostics;
 
    if (sessionDiagnostics != null) {
        Console.WriteLine(
                "  requests: {0}, failed: {1}",
                sessionDiagnostics.TotalRequestCounter.TotalCalls,
                sessionDiagnostics.TotalRequestCounter.FailedCalls);
 
        Console.WriteLine(
                "  unauthorized: {0}, writes: {1}",
                sessionDiagnostics.UnauthorizedRequestsCount,
                sessionDiagnostics.WriteCounter.TotalCalls);
    }
 
    foreach (OpcSubscription subscription in session.GetSubscriptions()) {
        OpcSubscriptionDiagnostics subscriptionDiagnostics = subscription.Diagnostics;
 
        if (subscriptionDiagnostics != null) {
            Console.WriteLine(
                    "  subscription {0}: queue overflows {1}",
                    subscription.Id,
                    subscriptionDiagnostics.MonitoringQueueOverflowCount);
        }
    }
}

// DOC
server.Diagnostics.IsEnabled = true;
server.Start();
