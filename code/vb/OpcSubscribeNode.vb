'DOC
Dim subscription As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/IsRunning", _
        AddressOf HandleDataChanged)

'DOC
Dim commands As OpcSubscribeDataChange() = New OpcSubscribeDataChange() { _
    New OpcSubscribeDataChange("ns=2;s=Machine/IsRunning", AddressOf HandleDataChanged), _
    New OpcSubscribeDataChange("ns=2;s=Machine/Job/Speed", AddressOf HandleDataChanged) _
}

Dim subscription As OpcSubscription = client.SubscribeNodes(commands)

'DOC
Private Shared Sub HandleDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    'Your code to execute on each data change.
    'The 'sender' variable contains the OpcMonitoredItem with the NodeId.
    Dim item As OpcMonitoredItem = CType(sender, OpcMonitoredItem)

    Console.WriteLine( _
            "Data Change from NodeId '{0}': {1}", _
            item.NodeId, _
            e.Item.Value)
End Sub


'DOC
Dim subscription As OpcSubscription = client.SubscribeEvent( _
        "ns=2;s=Machine", _
        AddressOf HandleEvent)

'DOC
Dim commands As OpcSubscribeEvent() = New OpcSubscribeEvent() { _
    New OpcSubscribeEvent("ns=2;s=Machine", AddressOf HandleEvent), _
    New OpcSubscribeEvent("ns=2;s=Machine/Job", AddressOf HandleEvent) _
}

Dim subscription As OpcSubscription = client.SubscribeNodes(commands)

'DOC
Private Sub HandleEvent(ByVal sender As Object, ByVal e As OpcEventReceivedEventArgs)
    'Your code to execute on each event raise.
End Sub


'DOC
subscription.PublishingInterval = 2000

'Always call apply changes after modifying the subscription; otherwise
'the server will not know the new subscription configuration.
subscription.ApplyChanges()

'DOC
Dim nodeIds As String() = { _
    "ns=2;s=Machine/IsRunning", _
    "ns=2;s=Machine/Job/Speed", _
    "ns=2;s=Machine/Diagnostics" _
}

'Create an (empty) subscription to which we will addd OpcMonitoredItems.
Dim subscription As OpcSubscription = client.SubscribeNodes()

For index As Integer = 0 To nodeIds.Length - 1
    'Create an OpcMonitoredItem for the NodeId.
    Dim item = New OpcMonitoredItem(nodeIds(index), OpcAttribute.Value)
    AddHandler item.DataChangeReceived, AddressOf HandleDataChanged

    'You can set your own values on the "Tag" property
    'that allows you to identify the source later.
    item.Tag = index

    'Set a custom sampling interval on the 
    'monitored item.
    item.SamplingInterval = 200

    'Add the item to the subscription.
    subscription.AddMonitoredItem(item)
Next

'After adding the items (or configuring the subscription), apply the changes.
subscription.ApplyChanges()

'DOC
Private Shared Sub HandleDataChanged( _
        ByVal sender As Object, _
        ByVal e As OpcDataChangeReceivedEventArgs)
    'The tag property contains the previously set value.
    Dim item As OpcMonitoredItem = CType(sender, OpcMonitoredItem)

    Console.WriteLine( _
        "Data Change from Index {0}: {1}", _
        item.Tag,
        e.Item.Value)
End Sub

