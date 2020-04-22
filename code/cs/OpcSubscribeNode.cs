// DOC
OpcSubscription subscription = client.SubscribeDataChange(
        "ns=2;s=Machine/IsRunning",
        HandleDataChanged);

// DOC
OpcSubscribeDataChange[] commands = new OpcSubscribeDataChange[] {
    new OpcSubscribeDataChange("ns=2;s=Machine/IsRunning", HandleDataChanged),
    new OpcSubscribeDataChange("ns=2;s=Machine/Job/Speed", HandleDataChanged)
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
            "Data Change from NodeId '{0}': {1}",
            item.NodeId,
            e.Item.Value);
}

// DOC
OpcSubscription subscription = client.SubscribeEvent(
        "ns=2;s=Machine",
        HandleEvent);

// DOC
OpcSubscribeEvent[] commands = new OpcSubscribeEvent[] {
    new OpcSubscribeEvent("ns=2;s=Machine", HandleEvent),
    new OpcSubscribeEvent("ns=2;s=Machine/Job", HandleEvent)
};
 
OpcSubscription subscription = client.SubscribeNodes(commands);

// DOC
private void HandleEvent(object sender, OpcEventReceivedEventArgs e)
{
    // Your code to execute on each event raise.
}

// DOC
subscription.PublishingInterval = 2000;
 
// Always call apply changes after modifying the subscription; otherwise
// the server will not know the new subscription configuration.
subscription.ApplyChanges();

// DOC
string[] nodeIds = {
    "ns=2;s=Machine/IsRunning",
    "ns=2;s=Machine/Job/Speed",
    "ns=2;s=Machine/Diagnostics"
};
 
// Create an (empty) subscription to which we will addd OpcMonitoredItems.
OpcSubscription subscription = client.SubscribeNodes();
 
for (int index = 0; index < nodeIds.Length; index++) {
    // Create an OpcMonitoredItem for the NodeId.
    var item = new OpcMonitoredItem(nodeIds[index], OpcAttribute.Value);
    item.DataChangeReceived += HandleDataChanged;
 
    // You can set your own values on the "Tag" property
    // that allows you to identify the source later.
    item.Tag = index;
 
    // Set a custom sampling interval on the 
    // monitored item.
    item.SamplingInterval = 200;
 
    // Add the item to the subscription.
    subscription.AddMonitoredItem(item);
}
 
// After adding the items (or configuring the subscription), apply the changes.
subscription.ApplyChanges();

// DOC
private static void HandleDataChanged(
        object sender,
        OpcDataChangeReceivedEventArgs e)
{
    // The tag property contains the previously set value.
    OpcMonitoredItem item = (OpcMonitoredItem)sender;
    
    Console.WriteLine(
            "Data Change from Index {0}: {1}",
            item.Tag,
            e.Item.Value);
}
