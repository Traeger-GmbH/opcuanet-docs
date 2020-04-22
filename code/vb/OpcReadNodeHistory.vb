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
