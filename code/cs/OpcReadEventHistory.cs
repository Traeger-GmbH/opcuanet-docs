// DOC
var severity = new OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity");

var filter = OpcFilter.Using(client)
        .FromEvents(OpcEventTypes.AlarmCondition)
        .Where(severity > OpcEventSeverity.Medium)
        .Select();

IEnumerable<OpcEvent> events = client.ReadEventHistory(
        startTime,
        endTime,
        "ns=2;s=Machine",
        filter);

foreach (var historicalEvent in events)
    Console.WriteLine("{0}: {1}", historicalEvent.Time, historicalEvent.Message);

// DOC
using (var eventNavigator = client.ReadEventHistory(
        startTime,
        endTime,
        10,
        "ns=2;s=Machine",
        filter)) {
    do {
        foreach (var historicalEvent in eventNavigator)
            Console.WriteLine("{0}: {1}", historicalEvent.Time, historicalEvent.Message);
    } while (eventNavigator.MoveNextPage());
}
