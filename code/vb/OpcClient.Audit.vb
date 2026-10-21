'DOC
Dim filter As OpcEventFilter = OpcFilter.Using(client) _
        .FromEvents( _
                OpcObjectTypes.GetNodeId(OpcObjectType.AuditCreateSessionEventType), _
                OpcObjectTypes.GetNodeId(OpcObjectType.AuditActivateSessionEventType)) _
        .Select()

client.SubscribeEvent(OpcObjectTypes.Server, filter, AddressOf HandleAuditEvent)
...
Private Shared Sub HandleAuditEvent( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim audit As OpcEvent = e.Event

    Console.WriteLine( _
            "{0:u} {1} {2}: {3}", _
            audit.Time, _
            audit.EventTypeId, _
            audit.SourceName, _
            audit.Message)
End Sub

