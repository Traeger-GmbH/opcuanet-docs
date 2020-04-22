'DOC
Dim subscription As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/IsRunning", _
        OpcDataChangeTrigger.StatusValueTimestamp, _
        AddressOf HandleDataChanged)

'DOC
Dim commands As OpcSubscribeDataChange() = New OpcSubscribeDataChange() { _
    New OpcSubscribeDataChange( _
            "ns=2;s=Machine/IsRunning", _
            OpcDataChangeTrigger.StatusValueTimestamp, _
            AddressOf HandleDataChanged), _
    New OpcSubscribeDataChange( _
            "ns=2;s=Machine/Job/Speed", _
            OpcDataChangeTrigger.StatusValueTimestamp, _
            AddressOf HandleDataChanged) _
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
            "Data Change from NodeId '{0}': {1} at {2}", _
            item.NodeId, _
            e.Item.Value, _
            e.Item.Value.SourceTimestamp)
End Sub


'DOC
Dim filter As OpcDataChangeFilter = New OpcDataChangeFilter()
filter.Trigger = OpcDataChangeTrigger.StatusValueTimestamp

Dim subscriptionA As OpcSubscription = client.SubscribeDataChange( _
        "ns=2;s=Machine/IsRunning", _
        filter, _
        AddressOf HandleDataChanged)

'or

Dim commands As OpcSubscribeDataChange() = New OpcSubscribeDataChange() { _
    New OpcSubscribeDataChange( _
            "ns=2;s=Machine/IsRunning", _
            filter, _
            AddressOf HandleDataChanged), _
    New OpcSubscribeDataChange( _
            "ns=2;s=Machine/Job/Speed", _
            filter, _
            HandleDataChanged) _
}

Dim subscriptionB As OpcSubscription = client.SubscribeNodes(commands)
