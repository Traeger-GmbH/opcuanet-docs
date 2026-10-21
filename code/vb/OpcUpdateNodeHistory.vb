'DOC
Dim results As OpcStatusCollection = client.CreateNodeHistory( _
        "ns=2;s=Machine/Job/Speed", _
        New OpcValue(1200, sourceTimestamp:=New DateTime(2017, 2, 16, 10, 0, 0)), _
        New OpcValue(1300, sourceTimestamp:=New DateTime(2017, 2, 16, 10, 5, 0)))

For Each result In results
    If result.IsBad Then
        Console.WriteLine("Failed to create value: {0}", result.Description)
    End If
Next

'DOC
Dim result As OpcStatus = client.ReplaceNodeHistory( _
        "ns=2;s=Machine/Job/Speed", _
        New OpcValue(1250, sourceTimestamp:=New DateTime(2017, 2, 16, 10, 0, 0)))

'DOC
Dim result As OpcStatus = client.UpdateNodeHistory( _
        "ns=2;s=Machine/Job/Speed", _
        New OpcValue(1250, sourceTimestamp:=New DateTime(2017, 2, 16, 10, 0, 0)))

'DOC
'Delete all values within the time window.
Dim results As OpcStatusCollection = client.DeleteNodeHistory( _
        "ns=2;s=Machine/Job/Speed", _
        startTime, _
        endTime)

'Delete the value of the timestamp specified.
Dim result As OpcStatus = client.DeleteNodeHistoryAtTime( _
        "ns=2;s=Machine/Job/Speed", _
        time:=New DateTime(2017, 2, 16, 10, 5, 0))
