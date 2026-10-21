// DOC
OpcEventFilter filter = OpcFilter.Using(client)
        .FromEvents(
                OpcObjectTypes.GetNodeId(OpcObjectType.AuditCreateSessionEventType),
                OpcObjectTypes.GetNodeId(OpcObjectType.AuditActivateSessionEventType))
        .Select();
 
client.SubscribeEvent(OpcObjectTypes.Server, filter, HandleAuditEvent);
...
private static void HandleAuditEvent(
        object sender,
        OpcEventReceivedEventArgs e)
{
    OpcEvent audit = e.Event;
 
    Console.WriteLine(
            "{0:u} {1} {2}: {3}",
            audit.Time,
            audit.EventTypeId,
            audit.SourceName,
            audit.Message);
}
