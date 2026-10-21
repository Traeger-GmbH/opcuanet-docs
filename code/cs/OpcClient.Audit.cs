// DOC
OpcEventFilter filter = OpcFilter.Using(client)
        .FromEvents(
            OpcObjectTypes.GetNodeId(OpcObjectType.AuditCreateSessionEventType),
            OpcObjectTypes.GetNodeId(OpcObjectType.AuditActivateSessionEventType))
        .Select();

client.SubscribeEvent(OpcObjectTypes.Server, filter, (sender, e) => {
    OpcEvent audit = e.Event;
    Console.WriteLine($"{audit.Time:u} {audit.EventTypeId} {audit.SourceName}: {audit.Message}");
});
