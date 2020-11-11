// DOC
private static readonly BlockingCollection<string> data = new BlockingCollection<string>();

// DOC
var processData = new CancellationTokenSource();
var processDataThread = new Thread(() => ProcessData(processData.Token));

processDataThread.Start();

// DOC
using (var client = new OpcClient("<address>")) {
    client.Connect();
    client.SubscribeDataChange("<nodeId>", HandleDataChanged);

    ...
}

// DOC
processData.Cancel();
processDataThread.Join();

// DOC
private static void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    data.Add(e.Item.Value.As<string>());
}

// DOC
private static void ProcessData(CancellationToken token)
{
    while (!token.IsCancellationRequested) {
        try {
            // Insert your data processing here.
            Debug.WriteLine(data.Take(token));
        }
        catch (OperationCanceledException) {
            // Can be ignored, because of the while loop will exit the method.
        }
    }
}
