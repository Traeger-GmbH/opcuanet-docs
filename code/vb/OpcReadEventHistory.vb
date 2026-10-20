'DOC
Dim severity = New OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity")

Dim filter = OpcFilter.Using(client) _
        .FromEvents(OpcEventTypes.AlarmCondition) _
        .Where(severity > OpcEventSeverity.Medium) _
        .Select()

Dim events As IEnumerable(Of OpcEvent) = client.ReadEventHistory( _
        startTime, _
        endTime, _
        "ns=2;s=Machine", _
        filter)

For Each historicalEvent In events
    Console.WriteLine("{0}: {1}", historicalEvent.Time, historicalEvent.Message)
Next

'DOC
Using eventNavigator = client.ReadEventHistory( _
        startTime, _
        endTime, _
        10, _
        "ns=2;s=Machine", _
        filter)
    Do
        For Each historicalEvent In eventNavigator
            Console.WriteLine("{0}: {1}", historicalEvent.Time, historicalEvent.Message)
        Next
    Loop While eventNavigator.MoveNextPage()
End Using
