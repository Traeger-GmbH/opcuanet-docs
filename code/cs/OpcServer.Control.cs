// DOC
private static nodesPerSession = new Dictionary<OpcNodeId, int>();

private static void HandleRequestValidating(object sender, OpcRequestValidatingEventArgs e)
{
    Console.Write(" -> Validating: " + e.Request.ToString());

    if (e.RequestType == OpcRequestType.AddNodes) {
        var sessionId = e.Context.SessionId;
        var request = (OpcAddNodesRequest)e.Request;

        lock (sender) {
            if (!nodesPerSession.TryGetValue(sessionId, out var count))
                nodesPerSession.Add(sessionId, count);

            count += request.Commands.Count;
            nodesPerSession[sessionId] = count;

            e.Cancel = (count >= 100);
        }
    }
}

// ...
server.RequestValidating += HandleRequestValidating;

// DOC
void HandleRequestValidated(object sender, OpcRequestValidatedEventArgs e)
{
    Console.Write(" -> Validated");
}

// ...

server.RequestValidated += HandleRequestValidated;

// DOC
private static void HandleRequestProcessed(object sender, OpcRequestProcessedEventArgs e)
{
    if (e.Response.Success)
        Console.WriteLine(" -> Processed!");
    else
        Console.WriteLine(" -> FAILED: {0}!", e.Exception?.Message ?? e.Response.ToString());
}

// ...

server.RequestProcessed += HandleRequestProcessed;

// DOC
private static void HandleRequestProcessing(object sender, OpcRequestProcessingEventArgs e)
{
    Console.Write("Processing: " + e.Request.ToString());
}

// ...

server.RequestProcessing += HandleRequestProcessing;

// DOC
server.RequestValidating += HandleRequestValidating;
...
private static readonly Dictionary<OpcNodeId, int> nodesPerSession
        = new Dictionary<OpcNodeId, int>();

private static void HandleRequestValidating(object sender, OpcRequestValidatingEventArgs e)
{
    Console.Write(" -> Validating: " + e.Request.ToString());

    if (e.RequestType == OpcRequestType.AddNodes) {
        var sessionId = e.Context.SessionId;
        var request = (OpcAddNodesRequest)e.Request;

        lock (nodesPerSession) {
            nodesPerSession.TryGetValue(sessionId, out var count);
            count += request.Commands.Count;

            // Reject the request in case it would exceed 100 AddNode commands per session.
            if (count > 100)
                e.Cancel = true;
            else
                nodesPerSession[sessionId] = count;
        }
    }
}

// DOC
server.RequestJudgement += HandleRequestJudgement;
...
private static void HandleRequestJudgement(object sender, OpcRequestJudgementEventArgs e)
{
    // Accept outdated requests, e.g. in case the clocks of client and server differ.
    if (e.Impeachment == OpcRequestImpeachment.Outdated)
        e.Dismiss = true;
}

// DOC
server.SessionCreated += (sender, e) => Console.WriteLine("Session created: {0}", e.Session.Name);
server.SessionClosing += (sender, e) => Console.WriteLine("Session closing: {0}", e.Session.Name);

server.SubscriptionCreated += (sender, e) => Console.WriteLine(
        "Subscription {0} created in session {1}", e.Subscription.Id, e.Subscription.Session.Name);

// DOC
// Announce the expected end of the maintenance to the clients.
server.EstimatedReturnTime = DateTime.UtcNow.AddMinutes(10);
server.Suspend();

// ... maintenance ...

server.Resume();
