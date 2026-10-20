// DOC
OpcSubscription subscription = client.SubscribeDataChange(
        "ns=2;s=Machine/IsRunning",
        HandleItemDataChanged);

subscription.DataChangeReceived += HandleSubscriptionDataChanged;
client.DataChangeReceived += HandleClientDataChanged;
...
private void HandleItemDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    if (e.Item.Value.Status.IsGood) {
        // Your code to process the value.

        // The handlers of the subscription and the client are not called.
        e.Handled = true;
    }
}

private void HandleSubscriptionDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    // Called for notifications not handled by the monitored item.
}

private void HandleClientDataChanged(object sender, OpcDataChangeReceivedEventArgs e)
{
    // Called for notifications not handled by the monitored item or the subscription.
}

// DOC
subscription.NotificationFailed += HandleSubscriptionNotificationFailed;
client.NotificationFailed += HandleClientNotificationFailed;
...
private void HandleSubscriptionNotificationFailed(object sender, OpcNotificationFailedEventArgs e)
{
    Console.WriteLine("Notification failed: {0}", e.Exception.Code);

    // The OpcClient.NotificationFailed event is not raised for this failure.
    e.Handled = true;
}

private void HandleClientNotificationFailed(object sender, OpcNotificationFailedEventArgs e)
{
    // The subscription is a null reference in case the message cannot be assigned to one.
    if (e.Subscription == null)
        Console.WriteLine("Notification failed: {0}", e.Exception.Message);
    else
        Console.WriteLine("Notification of subscription {0} failed: {1}", e.Subscription.Id, e.Exception.Message);
}

// DOC
var setup = new OpcSubscriptionSetup(
        new OpcMonitoredItem("ns=2;s=Machine/IsRunning", OpcAttribute.Value),
        new OpcMonitoredItem("ns=2;s=Machine/Job/Speed", OpcAttribute.Value)) {
    PublishingInterval = 1000,
    DataChangeReceived = HandleDataChanged,
    NotificationFailed = HandleNotificationFailed
};

// Creates the subscription including its monitored items on the server.
OpcSubscription subscription = client.CreateSubscription(setup);
