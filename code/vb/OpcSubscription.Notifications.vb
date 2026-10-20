'DOC
Dim subscription As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/IsRunning", _
        AddressOf HandleItemDataChanged)

AddHandler subscription.DataChangeReceived, AddressOf HandleSubscriptionDataChanged
AddHandler client.DataChangeReceived, AddressOf HandleClientDataChanged
...
Private Sub HandleItemDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    If e.Item.Value.Status.IsGood Then
        'Your code to process the value.

        'The handlers of the subscription and the client are not called.
        e.Handled = True
    End If
End Sub

Private Sub HandleSubscriptionDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    'Called for notifications not handled by the monitored item.
End Sub

Private Sub HandleClientDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    'Called for notifications not handled by the monitored item or the subscription.
End Sub

'DOC
AddHandler subscription.NotificationFailed, AddressOf HandleSubscriptionNotificationFailed
AddHandler client.NotificationFailed, AddressOf HandleClientNotificationFailed
...
Private Sub HandleSubscriptionNotificationFailed(ByVal sender As Object, ByVal e As OpcNotificationFailedEventArgs)
    Console.WriteLine("Notification failed: {0}", e.Exception.Code)

    'The OpcClient.NotificationFailed event is not raised for this failure.
    e.Handled = True
End Sub

Private Sub HandleClientNotificationFailed(ByVal sender As Object, ByVal e As OpcNotificationFailedEventArgs)
    'The subscription is Nothing in case the message cannot be assigned to one.
    If e.Subscription Is Nothing Then
        Console.WriteLine("Notification failed: {0}", e.Exception.Message)
    Else
        Console.WriteLine("Notification of subscription {0} failed: {1}", e.Subscription.Id, e.Exception.Message)
    End If
End Sub

'DOC
Dim setup = New OpcSubscriptionSetup( _
        New OpcMonitoredItem("ns=2;s=Machine/IsRunning", OpcAttribute.Value), _
        New OpcMonitoredItem("ns=2;s=Machine/Job/Speed", OpcAttribute.Value)) With {
    .PublishingInterval = 1000,
    .DataChangeReceived = AddressOf HandleDataChanged,
    .NotificationFailed = AddressOf HandleNotificationFailed
}

'Creates the subscription including its monitored items on the server.
Dim subscription As OpcSubscription = client.CreateSubscription(setup)
