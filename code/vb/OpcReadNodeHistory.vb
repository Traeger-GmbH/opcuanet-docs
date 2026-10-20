'DOC
Dim startTime = New DateTime(2017, 2, 16, 10, 0, 0)
Dim history = client.ReadNodeHistory( _
        startTime, Nothing, "ns=2;s=Machine/Job/Speed")

'DOC
Dim endTime = New DateTime(2017, 2, 16, 15, 0, 0)
Dim history = client.ReadNodeHistory( _
        Nothing, endTime, "ns=2;s=Machine/Job/Speed")

'DOC
Dim startTime = New DateTime(2017, 2, 16, 10, 0, 0)
Dim endTime = New DateTime(2017, 2, 16, 15, 0, 0)

Dim history = client.ReadNodeHistory( _
        startTime, endTime, "ns=2;s=Machine/Job/Speed")

'DOC
For Each value In history
    Console.WriteLine( _
            "{0}: {1}",
            value.Timestamp,
            value)
Next

'DOC
Dim historyNavigator = client.ReadNodeHistory( _
        10, "ns=2;s=Machine/Job/Speed")

'DOC
Dim startTime = New DateTime(2017, 2, 16, 15, 0, 0)
Dim historyNavigator = client.ReadNodeHistory( _
        startTime, 10, "ns=2;s=Machine/Job/Speed")

'DOC
Dim endTime = New DateTime(2017, 2, 16, 15, 0, 0)
Dim historyNavigator = client.ReadNodeHistory( _
        Nothing, endTime, 10, "ns=2;s=Machine/Job/Speed")

'DOC
Dim startTime = New DateTime(2017, 2, 16, 10, 0, 0)
Dim endTime = New DateTime(2017, 2, 16, 15, 0, 0)

Dim historyNavigator = client.ReadNodeHistory( _
        startTime, endTime, 10, "ns=2;s=Machine/Job/Speed")

'DOC
Do
    For Each value In historyNavigator
        Console.WriteLine( _
                "{0}: {1}", _
                value.Timestamp, _
                value)
    Next
Loop While historyNavigator.MoveNextPage()

historyNavigator.Close()

'DOC
Using historyNavigator
    Do
        For Each value In historyNavigator
            Console.WriteLine( _
                    "{0}: {1}", _
                    value.Timestamp, _
                    value)
        Next
    Loop While historyNavigator.MoveNextPage()
End Using

'DOC
Dim minSpeed = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Minimum, _
        "ns=2;s=Machine/Job/Speed")

'DOC
Dim avgSpeed = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Average, _
        "ns=2;s=Machine/Job/Speed")

'DOC
Dim maxSpeed = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Maximum, _
        "ns=2;s=Machine/Job/Speed")

'DOC
Dim startTime = New DateTime(2017, 2, 16, 10, 0, 0)
Dim history = client.ReadNodeHistory( _
        startTime, CType(Nothing, DateTime?), "ns=2;s=Machine/Job/Speed")

'DOC
'Read the average value of each hour (3600000 milliseconds) within the time window.
Dim avgSpeedPerHour = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Average, _
        3600000, _
        "ns=2;s=Machine/Job/Speed")

'DOC
Dim times = New DateTime() { _
    New DateTime(2017, 2, 16, 10, 0, 0), _
    New DateTime(2017, 2, 16, 12, 0, 0) _
}

Dim speeds = client.ReadNodeHistoryAtTime(times, "ns=2;s=Machine/Job/Speed")

For Each value In speeds
    Console.WriteLine("{0}: {1}", value.Timestamp, value)
Next

'DOC
Dim modifiedSpeeds = client.ReadNodeHistoryModified( _
        startTime, _
        endTime, _
        "ns=2;s=Machine/Job/Speed")

For Each value In modifiedSpeeds
    Console.WriteLine( _
            "{0}: {1} ({2} at {3} by {4})", _
            value.Timestamp, _
            value, _
            value.ModificationType, _
            value.ModificationTime, _
            value.ModificationUserName)
Next
