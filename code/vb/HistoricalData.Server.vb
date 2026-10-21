'DOC
'"Me" points to the Node-Manager of the node.
Dim machineIsRunningHistorian = New OpcNodeHistorian(Me, machineIsRunningNode)

'DOC
machineIsRunningNode.AccessLevel = machineIsRunningNode.AccessLevel Or OpcAccessLevel.HistoryReadOrWrite
machineIsRunningNode.UserAccessLevel = machineIsRunningNode.UserAccessLevel Or OpcAccessLevel.HistoryReadOrWrite

machineIsRunningNode.IsHistorizing = True

'DOC
machineIsRunningHistorian.AutoUpdateHistory = True

'DOC
machineIsRunningNode.BeforeApplyChanges = AddressOf HandleBeforeApplyChanges;
...
Private Sub HandleBeforeApplyChanges(ByVal sender As Object, ByVal e As EventArgs)
    'Update (modified) Node History here.
End Sub


'DOC
Protected Overrides Function RetrieveNodeHistoryProvider(ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node = machineIsRunnigNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function ReadHistory( _
        ByVal context As OpcContext, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal options As OpcReadHistoryOptions) As IEnumerable(Of OpcHistoryValue)
    'Read (modified) Node History here.
End Function


'DOC
Protected Overrides Function ReadHistory( _
        ByVal node As IOpcNode, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal options As OpcReadHistoryOptions) As IEnumerable(Of OpcHistoryValue)
    'Read (modified) Node History here.
End Function


'DOC
Protected Overrides Function RetrieveNodeHistoryProvider(ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node = machineIsRunnigNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function CreateHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Create (modified) Node History here.
End Function


'DOC
Protected Overrides Function CreateHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Create (modified) Node History here.
End Function


'DOC
Protected Overrides Function RetrieveNodeHistoryProvider(ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node = machineIsRunnigNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function DeleteHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal times As IEnumerable(Of DateTime)) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function
 
Public Function DeleteHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function
 
Public Function DeleteHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal options As OpcDeleteHistoryOptions) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function




'DOC
Protected Overrides Function DeleteHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal times As IEnumerable(Of DateTime)) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function
 
Protected Overrides Function DeleteHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function
 
Protected Overrides Function DeleteHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal options As OpcDeleteHistoryOptions) As OpcStatusCollection
    'Delete Node History entries and add them to the modified Node History here.
End Function




'DOC
Protected Overrides Function RetrieveNodeHistoryProvider(ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node = machineIsRunnigNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function ReplaceHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Replace Node History entries and add them to the modified Node History here.
End Function


'DOC
Protected Overrides Function ReplaceHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Replace Node History entries and add them to the modified Node History here.
End Function


'DOC
Protected Overrides Function RetrieveNodeHistoryProvider(ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node = machineIsRunnigNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function UpdateHistory( _
        ByVal context As OpcContext, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Update (modified) Node History entries here.
End Function


'DOC
Protected Overrides Function UpdateHistory( _
        ByVal node As IOpcNode, _
        ByVal modificationInfo As OpcHistoryModificationInfo, _
        ByVal values As OpcValueCollection) As OpcStatusCollection
    'Update (modified) Node History entries here.
End Function


'DOC
Dim history = New OpcNodeHistory(Of OpcHistoryValue)()

'DOC
Dim modifiedHistory = New OpcNodeHistory(Of OpcModifiedHistoryValue)()

'DOC
Dim results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count)

For index As Integer = 0 To values.Count - 1
    Dim result = results(index)
    Dim value = OpcHistoryValue.Create(values(index))

    If MatchesValueType(value) Then
        If history.Contains(value.Timestamp) Then
            result.Update(OpcStatusCode.BadEntryExists)
        Else
            history.Add(value)

            Dim modifiedValue = value.CreateModified(modificationInfo)
            modifiedHistory.Add(modifiedValue)

            result.Update(OpcStatusCode.GoodEntryInserted)
        End If
    Else
        result.Update(OpcStatusCode.BadTypeMismatch)
    End If
Next

Return results



'DOC
Dim results = OpcStatusCollection.Create(OpcStatusCode.Good, times.Count())

Dim index As Integer = 0

For Each time In times
    Dim result = results(Math.Min(System.Threading.Interlocked.Increment(index), index - 1))

    If Me.history.Contains(time) Then
        Dim value = Me.history(time)
        Me.history.RemoveAt(time)

        Dim modifiedValue = value.CreateModified(modificationInfo)
        Me.modifiedHistory.Add(modifiedValue)
    Else
        result.Update(OpcStatusCode.BadNoEntryExists)
    End If
Next

Return results


'DOC
Dim results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count)

For index As Integer = 0 To values.Count - 1
    Dim timestamp = OpcHistoryValue.Create(values(index)).Timestamp
    Dim result = results(index)

    If history.Contains(timestamp) Then
        Dim value = history(timestamp)
        history.RemoveAt(timestamp)

        Dim modifiedValue = value.CreateModified(modificationInfo)
        modifiedHistory.Add(modifiedValue)
    Else
        result.Update(OpcStatusCode.BadNoEntryExists)
    End If
Next

Return results


'DOC
Dim results = New OpcStatusCollection()
Dim isModified As Boolean = (options And OpcDeleteHistoryOptions.Modified) _
        = OpcDeleteHistoryOptions.Modified

If isModified Then
    modifiedHistory.RemoveRange(startTime, endTime)
Else
    Dim values = history.Enumerate(startTime, endTime).ToArray()
    history.RemoveRange(startTime, endTime)

    For index As Integer = 0 To values.Length - 1
        Dim value = values(index)
        modifiedHistory.Add(value.CreateModified(modificationInfo))

        results.Add(OpcStatusCode.Good)
    Next
End If

Return results



'DOC
Dim results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count)

For index As Integer = 0 To values.Count - 1
    Dim result = results(index)
    Dim value = OpcHistoryValue.Create(values(index))

    If Me.MatchesNodeValueType(value) Then
        If Me.history.Contains(value.Timestamp) Then
            Dim oldValue = Me.history(value.Timestamp)
            history.Replace(value)

            Dim modifiedValue = oldValue.CreateModified(modificationInfo)
            modifiedHistory.Add(modifiedValue)

            result.Update(OpcStatusCode.GoodEntryReplaced)
        Else
            result.Update(OpcStatusCode.BadNoEntryExists)
        End If
    Else
        result.Update(OpcStatusCode.BadTypeMismatch)
    End If
Next

Return results



'DOC
Dim results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count)

For index As Integer = 0 To values.Count - 1
    Dim result = results(index)
    Dim value = OpcHistoryValue.Create(values(index))

    If MatchesValueType(value) Then
        If history.Contains(value.Timestamp) Then
            Dim oldValue = Me.history(value.Timestamp)
            history.Replace(value)

            Dim modifiedValue = oldValue.CreateModified(modificationInfo)
            modifiedHistory.Add(modifiedValue)

            result.Update(OpcStatusCode.GoodEntryReplaced)
        Else
            history.Add(value)

            Dim modifiedValue = value.CreateModified(modificationInfo)            
            modifiedHistory.Add(modifiedValue)

            result.Update(OpcStatusCode.GoodEntryInserted)
        End If
    Else
        result.Update(OpcStatusCode.BadTypeMismatch)
    End If
Next

Return results



'DOC
Dim isModified As Boolean = (options And OpcReadHistoryOptions.Modified) _
        = OpcReadHistoryOptions.Modified

If isModified Then
    Return modifiedHistory _
            .Enumerate(startTime, endTime) _
            .Cast(Of OpcHistoryValue)() _
            .ToArray()
End If

Return history _
        .Enumerate(startTime, endTime) _
        .ToArray()

'DOC
Protected Overrides Function RetrieveNodeHistoryProvider( _
        ByVal node As IOpcNode) As IOpcNodeHistoryProvider
    If node Is machineIsRunningNode Then
        Return machineIsRunningHistorian
    End If

    Return MyBase.RetrieveNodeHistoryProvider(node)
End Function

'DOC
Public Function ReadHistory( _
        ByVal context As OpcContext, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal timeFlowsBackward As Boolean, _
        ByVal maxCount As UInteger?, _
        ByVal options As OpcReadHistoryOptions) As IEnumerable(Of OpcHistoryValue) _
        Implements IOpcNodeHistoryProvider.ReadHistory
    'Read (modified) Node History here.
End Function

'DOC
Protected Overrides Function ReadHistory( _
        ByVal context As OpcContext, _
        ByVal node As IOpcNode, _
        ByVal startTime As DateTime?, _
        ByVal endTime As DateTime?, _
        ByVal timeFlowsBackward As Boolean, _
        ByVal maxCount As UInteger?, _
        ByVal options As OpcReadHistoryOptions) As IEnumerable(Of OpcHistoryValue)
    'Read (modified) Node History here.
End Function


'DOC
Dim isModified As Boolean = (options And OpcReadHistoryOptions.Modified) _
        = OpcReadHistoryOptions.Modified

Dim values As OpcHistoryValue()

If isModified Then
    values = modifiedHistory _
            .Enumerate(startTime, endTime) _
            .Cast(Of OpcHistoryValue)() _
            .ToArray()
Else
    values = history _
            .Enumerate(startTime, endTime) _
            .ToArray()
End If

'Return the values in reverse order in case the time flows backward.
If timeFlowsBackward Then
    Array.Reverse(values)
End If

Return values

'DOC
Public Class MachineEventHistory
    Implements IOpcEventHistoryProvider

    Private ReadOnly events As New List(Of OpcEvent)()

    Public Sub New(ByVal node As OpcObjectNode)
        Me.Node = node
        Me.Node.IsHistorizingEvents = True
    End Sub

    Public ReadOnly Property Node As OpcObjectNode Implements IOpcEventHistoryProvider.Node

    Public Sub Add(ByVal historyEvent As OpcEvent)
        SyncLock Me.events
            Me.events.Add(historyEvent)
        End SyncLock
    End Sub

    Public Function ReadEventHistory( _
            ByVal context As OpcContext, _
            ByVal filter As OpcEventFilter, _
            ByVal startTime As DateTime?, _
            ByVal endTime As DateTime?, _
            ByVal timeFlowsBackward As Boolean, _
            ByVal maxCount As UInteger?) As IEnumerable(Of OpcEvent) _
            Implements IOpcEventHistoryProvider.ReadEventHistory
        'In case the time flows backward the start time is the later one.
        Dim from As DateTime? = If(timeFlowsBackward, endTime, startTime)
        Dim [to] As DateTime? = If(timeFlowsBackward, startTime, endTime)

        Dim result = New List(Of OpcEvent)()

        SyncLock Me.events
            For Each item In Me.events
                If IsSet(from) AndAlso item.Time < from.Value Then Continue For
                If IsSet([to]) AndAlso item.Time > [to].Value Then Continue For

                result.Add(item)
            Next
        End SyncLock

        If timeFlowsBackward Then
            result.Reverse()
        End If

        If maxCount.HasValue AndAlso result.Count > maxCount.Value Then
            result.RemoveRange(CInt(maxCount.Value), result.Count - CInt(maxCount.Value))
        End If

        Return result
    End Function

    Private Shared Function IsSet(ByVal time As DateTime?) As Boolean
        Return time.HasValue AndAlso time.Value <> DateTime.MinValue
    End Function
End Class



'DOC
Protected Overrides Function RetrieveEventHistoryProvider( _
        ByVal node As IOpcNode) As IOpcEventHistoryProvider
    If node Is Me.machineEventHistory.Node Then
        Return Me.machineEventHistory
    End If

    Return MyBase.RetrieveEventHistoryProvider(node)
End Function

'DOC
activatedEvent.Message = "Machine started"
activatedEvent.ReportEvent(Me.SystemContext)

'Stores a snapshot of the event in the event history.
Me.machineEventHistory.Add(activatedEvent.CreateEvent(Me.SystemContext))
