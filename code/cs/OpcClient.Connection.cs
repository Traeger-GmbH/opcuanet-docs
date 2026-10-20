// DOC
client.UseBreakDetection = true;    // Default: true
client.KeepAlive.Interval = 2000;   // Default: 5000
client.ReconnectTimeout = 5000;     // Default: 10000

// DOC
client.Reconnecting += HandleReconnecting;
client.Reconnected += HandleReconnected;
client.BreakDetected += HandleBreakDetected;
...
private void HandleReconnecting(object sender, EventArgs e)
{
    Console.WriteLine("Connection lost, trying to reconnect.");
}

private void HandleReconnected(object sender, EventArgs e)
{
    Console.WriteLine("Connection restored using the existing session.");
}

private void HandleBreakDetected(object sender, EventArgs e)
{
    Console.WriteLine("Connection lost, no automatic reconnect.");
}

// DOC
client.KeepAlive.Updated += HandleKeepAliveUpdated;
...
private void HandleKeepAliveUpdated(object sender, EventArgs e)
{
    var keepAlive = (OpcClientKeepAlive)sender;

    if (keepAlive.IsActive)
        Console.WriteLine("Server state: {0}, server time: {1}", keepAlive.ServerState, keepAlive.Time);
    else
        Console.WriteLine("Keep alive failed: {0}", keepAlive.Status);
}
