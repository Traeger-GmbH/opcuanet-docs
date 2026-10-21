'DOC
Dim subscription As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/IsRunning", _
        AddressOf HandleItemDataChanged)

AddHandler subscription.DataChangeReceived, AddressOf HandleSubscriptionDataChanged
AddHandler client.DataChangeReceived, AddressOf HandleClientDataChanged
...
Private Shared Sub HandleItemDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    If e.Item.Value.Status.IsGood Then
        'Your code to process the value.

        'The handlers of the subscription and the client are not called.
        e.Handled = True
    End If
End Sub

Private Shared Sub HandleSubscriptionDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    'Called for notifications not handled by the monitored item.
End Sub

Private Shared Sub HandleClientDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    'Called for notifications not handled by the monitored item or the subscription.
End Sub




'DOC
AddHandler subscription.NotificationFailed, AddressOf HandleSubscriptionNotificationFailed
AddHandler client.NotificationFailed, AddressOf HandleClientNotificationFailed
...
Private Shared Sub HandleSubscriptionNotificationFailed( _
        ByVal sender As Object, _
        ByVal e As OpcNotificationFailedEventArgs)
    Console.WriteLine("Notification failed: {0}", e.Exception.Code)

    'The OpcClient.NotificationFailed event is not raised for this failure.
    e.Handled = True
End Sub

Private Shared Sub HandleClientNotificationFailed( _
        ByVal sender As Object, _
        ByVal e As OpcNotificationFailedEventArgs)
    'The subscription is Nothing in case the message cannot be assigned to one.
    If e.Subscription Is Nothing Then
        Console.WriteLine("Notification failed: {0}", e.Exception.Message)
    Else
        Console.WriteLine( _
                "Notification of subscription {0} failed: {1}", _
                e.Subscription.Id, _
                e.Exception.Message)
    End If
End Sub




'DOC
Dim setup = New OpcSubscriptionSetup( _
        New OpcMonitoredItem("ns=2;s=Machine/IsRunning", OpcAttribute.Value), _
        New OpcMonitoredItem("ns=2;s=Machine/Job/Speed", OpcAttribute.Value)) With { _
    .PublishingInterval = 1000, _
    .DataChangeReceived = AddressOf HandleDataChanged, _
    .NotificationFailed = AddressOf HandleNotificationFailed _
}

'Creates the subscription including its monitored items on the server.
Dim subscription As OpcSubscription = client.CreateSubscription(setup)
