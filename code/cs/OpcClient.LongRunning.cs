// DOC
var client = new OpcClient("opc.tcp://localhost:4840/");
client.ReconnectTimeout = 5000;

while (true) {
    try {
        client.Connect();
        break;
    }
    catch (OpcException ex) {
        Console.WriteLine("Connect failed: {0}", ex.Code);
        Thread.Sleep(5000);
    }
}

// DOC
// Subscribe once after Connect: the client recreates the subscription for a new session.
OpcSubscription subscription = client.SubscribeDataChange(
        "ns=2;s=Machine/Speed",
        HandleDataChanged);
...
private void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    Console.WriteLine("Speed: {0}", e.Item.Value);
}

// DOC
client.StateChanged += HandleStateChanged;
...
private void HandleStateChanged(object sender, OpcClientStateChangedEventArgs e)
{
    Console.WriteLine("{0} -> {1}", e.OldState, e.NewState);

    if (e.OldState == OpcClientState.Reconnecting && e.NewState == OpcClientState.Connected)
        Console.WriteLine("New session, the subscriptions have been recreated.");
}

// DOC
client.Connected += HandleConnected;
...
private OpcNodeId[] nodeIds = { "ns=2;s=Machine/Speed", "ns=2;s=Machine/Temperature" };
private OpcNodeIdCollection registeredNodeIds;

private void HandleConnected(object sender, EventArgs e)
{
    // Registered node ids are only valid in the session they have been registered in.
    this.registeredNodeIds = ((OpcClient)sender).RegisterNodes(this.nodeIds);
}

// DOC
client.ReconnectTimeout = 0;
client.BreakDetected += HandleBreakDetected;
...
private void HandleBreakDetected(object sender, EventArgs e)
{
    var client = (OpcClient)sender;

    // Decide on your own when and how to connect again.
    Task.Run(() => {
        Thread.Sleep(10000);
        client.Connect();
    });
}
