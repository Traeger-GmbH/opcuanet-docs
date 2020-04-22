// DOC
OpcSubscription subscription = client.SubscribeDataChange(
        "ns=2;s=Machine/IsRunning",
        OpcDataChangeTrigger.StatusValueTimestamp,
        HandleDataChanged);

// DOC
OpcSubscribeDataChange[] commands = new OpcSubscribeDataChange[] {
    new OpcSubscribeDataChange(
            "ns=2;s=Machine/IsRunning",
            OpcDataChangeTrigger.StatusValueTimestamp,
            HandleDataChanged),
    new OpcSubscribeDataChange(
            "ns=2;s=Machine/Job/Speed",
            OpcDataChangeTrigger.StatusValueTimestamp,
            HandleDataChanged)
};
 
OpcSubscription subscription = client.SubscribeNodes(commands);

// DOC
private static void HandleDataChanged(
        object sender,
        OpcDataChangeReceivedEventArgs e)
{
    // Your code to execute on each data change.
    // The 'sender' variable contains the OpcMonitoredItem with the NodeId.
    OpcMonitoredItem item = (OpcMonitoredItem)sender;

    Console.WriteLine(
            "Data Change from NodeId '{0}': {1} at {2}",
            item.NodeId,
            e.Item.Value,
            e.Item.Value.SourceTimestamp);
}

// DOC
OpcDataChangeFilter filter = new OpcDataChangeFilter();
filter.Trigger = OpcDataChangeTrigger.StatusValueTimestamp;
 
OpcSubscription subscriptionA = client.SubscribeDataChange(
        "ns=2;s=Machine/IsRunning",
        filter,
        HandleDataChanged);
 
// or
 
OpcSubscribeDataChange[] commands = new OpcSubscribeDataChange[] {
    new OpcSubscribeDataChange(
            "ns=2;s=Machine/IsRunning",
            filter,
            HandleDataChanged),
    new OpcSubscribeDataChange(
            "ns=2;s=Machine/Job/Speed",
            filter,
            HandleDataChanged)
};
 
OpcSubscription subscriptionB = client.SubscribeNodes(commands);
