' DOC
Dim filter As OpcEventFilter = OpcFilter.Using(client) _
        .FromEvents(
            OpcObjectTypes.GetNodeId(OpcObjectType.AuditCreateSessionEventType),
            OpcObjectTypes.GetNodeId(OpcObjectType.AuditActivateSessionEventType)) _
        .Select()

client.SubscribeEvent(OpcObjectTypes.Server, filter,
    Sub(sender, e)
        Dim audit As OpcEvent = e.Event
        Console.WriteLine($"{audit.Time:u} {audit.EventTypeId} {audit.SourceName}: {audit.Message}")
    End Sub)
