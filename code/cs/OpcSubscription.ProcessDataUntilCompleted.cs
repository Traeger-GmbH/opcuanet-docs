// DOC
private static readonly BlockingCollection<string> data = new BlockingCollection<string>();

// DOC
var processDataThread = new Thread(ProcessData);
processDataThread.Start();

// DOC
using (var client = new OpcClient("<address>")) {
    client.Connect();
    client.SubscribeDataChange("<nodeId>", HandleDataChanged);

    ...
}

// DOC
data.CompleteAdding();
processDataThread.Join();

// DOC
private static void HandleDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    data.Add(e.Item.Value.As<string>());
}

// DOC
private static void ProcessData()
{
    // Using 'GetConsumingEnumerable' removes an item from the collection, consumes it through
    // the foreach loop and blocks until another item is/has being added to collection or
    // 'CompleteAdding' is used to signal the end of the data processing.
    foreach (var item in data.GetConsumingEnumerable()) {
        // Insert your data processing here.
    }
}
