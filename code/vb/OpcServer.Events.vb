' DOC
Dim server As New OpcServer(...)
' ...

server.ReportEvent( _
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation.")

' Same usage as before + arguments support.
server.ReportEvent( _
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation at machine {0}.", _
        machineId)

' Same usage as before + source node.
server.ReportEvent( _
        sourceNode, _
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation.")

' Same usage as before + arguments support.
server.ReportEvent( _
        sourceNode, _
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation at machine {0}.", _
        machineId)

' Same usage as before + explicit source information.
server.ReportEvent( _
        sourceNodeId, _
        sourceNodeName, 
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation.")

' Same usage as before + arguments support.
server.ReportEvent( _
        sourceNodeId, _
        sourceNodeName, _
        OpcEventSeverity.Medium, _
        "Recognized a medium urgent situation at machine {0}.", _
        machineId)

' DOC
Dim activatedEvent As New OpcEventNode(machineOne, "Activated")

' DOC
machineOne.AddNotifier(Me.SystemContext, activatedEvent)

' DOC
activatedEvent.SourceNodeId = sourceNodeId
activatedEvent.SourceName = sourceNodeName
activatedEvent.Severity = OpcEventSeverity.Medium
activatedEvent.Message = "Recognized a medium urgent situation."

' DOC
' Server generated value to identify a specific Event
activatedEvent.EventId = ...

' The time the event occured
activatedEvent.Time = ...

' The time the event has been received by the underlaying system / device
activatedEvent.ReceiveTime = ...

' DOC
activatedEvent.ReportEvent(Me.SystemContext)

' DOC
AddHandler machineOne.QueryEventsCallback, Sub(context, events)
    ' Ensure that an re-entrance upon notifier cross-references will not add
    ' events to the collection which are already stored in.
    If events.Count <> 0 Then Return
    
    events.Add(activatedEvent.CreateEvent(context))
End Sub


' DOC
Dim maintenanceEvent As New OpcConditionNode(machineOne, "Maintenance")

' Interesting for a client yes or no
maintenanceEvent.IsRetained = True ' = default

' Condition is enabled or disabled
maintenanceEvent.IsEnabled ' use ChangeIsEnabled(...)

' Status of the source the condition is based upon
maintenanceEvent.Quality = ...

' DOC
' Identifier of the user who supplied the Comment
maintenanceEvent.ClientUserId = ...

' Last comment provided by a user
maintenanceEvent.Comment = ...

' DOC 
' Uses a new GUID as BranchId
Dim maintenanceBranchA As maintenanceEvent.CreateBranch(Me.SystemContext)

' Uses a custom NodeId as BranchId
Dim maintenanceBranchB As maintenanceEvent.CreateBranch(Me.SystemContext, New OpcNodeId(10001))

...

' Identifies the branch of the event
maintenanceEvent.BranchId = ...

' Previous severity of the branch
maintenanceEvent.LastSeverity = ...

' DOC
Dim outOfMaterial As New OpcDialogConditionNode(machineOne, "MaterialAlert")

outOfMaterial.Message = "Out of Material" ' Generic event message
outOfMaterial.Prompt = "The machine is out of material. Refill material supply to continue."
outOfMaterial.ResponseOptions = New OpcText() {"Continue", "Cancel"}
outOfMaterial.DefaultResponse = 0 ' Index of ResponseOption to use
outOfMaterial.CancelResponse = 1 ' Index of ResponseOption to use
outOfMaterial.OkResponse = 0 ' Index of ResponseOption to use

' DOC
AddHandler outOfMaterial.RespondCallback, AddressOf HandleOutOfMaterialResponse

...

Private Function HandleOutOfMaterialResponse( _
        context As OpcNodeContext(Of OpcDialogConditionNode), _
        selectedResponse As Integer) As OpcStatusCode
    ' Handle the response
    If context.Node.OkResponse = selectedResponse Then
        ContinueJob()
    Else
        CancelJob()
    End If

    ' Apply the response
    context.Node.RespondDialog(context, response)

    Return OpcStatusCode.Good
End Function

' DOC
Dim outOfProcessableBounds As New OpcAcknowledgeableConditionNode(machineOne, "OutOfBoundsAlert")

' Define the condition as: Needs to be acknowledged
outOfProcessableBounds.ChangeIsAcked(Me.SystemContext, False)

' Define the condition as: Needs to be confirmed
outOfProcessableBounds.ChangeIsConfirmed(Me.SystemContext, False)

' DOC
If outOfProcessableBounds.IsAcked Then
    ...
End If

If outOfProcessableBounds.IsConfirmed Then
    ...
End If

' DOC
Dim overheating As New OpcAlarmConditionNode(machineOne, "OverheatingAlert")
Dim idle As New OpcAlarmConditionNode(machineOne, "IdleAlert")
'...
overheating.ChangeIsActive(Me.SystemContext, True)
idle.ChangeIsActive(Me.SystemContext, True)

...

If overheating.IsActive Then
    CancelJob()
End If

If Not idle.IsActive Then
    ProcessJob()
ElseIf idle.IsSuppressed Then
    SimulateJob()
End If

' DOC
Dim x As New OpcDiscreteAlarmNode(machineOne, "discreteAlert")
Dim y As New OpcOffNormalAlarmNode(machineOne, "offNormalAlert")
Dim z As New OpcTripAlarmNode(machineOne, "tripAlert")

' DOC
Dim positionLimit As New OpcLimitAlarmNode(
        machineOne, "PositionLimit", OpcLimitAlarmStates.HighHigh Or OpcLimitAlarmStates.LowLow)

positionLimit.HighHighLimit = 120 ' e.g. mm
positionLimit.LowLowLimit = 0 ' e.g. mm

' DOC
machineOne.QueryEventsCallback =
    Sub(context, events)
        ' Ensure that an re-entrance upon notifier cross-references will not add
        ' events to the collection which are already stored in.
        If events.Count <> 0 Then
            Return
        End If

        events.Add(activatedEvent.CreateEvent(context))
    End Sub

' DOC
' Applies the changes of the event node and sends the event to all subscribers.
server.ReportEvent(activatedEvent)

' DOC
Dim maintenanceEvent = New OpcConditionNode(machineOne, "Maintenance")

' Default: EnabledOnly for an OpcConditionNode,
' ActiveOrUnacknowledged for acknowledgeable conditions and alarms.
maintenanceEvent.IsRetainedPolicy = OpcConditionRetainPolicy.Manual

' Interesting for a client yes or no (only kept in case of the Manual policy)
maintenanceEvent.IsRetained = True

' Status of the source the condition is based upon
maintenanceEvent.Quality = OpcStatusCode.Good

' DOC
' Use the Change methods once the node has been added to the address space.
maintenanceEvent.ChangeIsEnabled(Me.SystemContext, False)
maintenanceEvent.ChangeComment(Me.SystemContext, "Planned maintenance")

' Reports an event whenever one of the Change methods completes.
maintenanceEvent.AutoReportChanges = True

' DOC
' Uses a new GUID as BranchId
Dim maintenanceBranchA = maintenanceEvent.CreateBranch(Me.SystemContext)

' Uses a custom NodeId as BranchId
Dim maintenanceBranchB = maintenanceEvent.CreateBranch(Me.SystemContext, New OpcNodeId(10001))

' DOC
outOfMaterial.RespondCallback = AddressOf Me.HandleOutOfMaterialResponse
outOfMaterial.ActivateDialog(Me.SystemContext)
'...
Private Function HandleOutOfMaterialResponse(
        context As OpcNodeContext(Of OpcDialogConditionNode),
        selectedResponse As Integer) As OpcStatusCode
    ' Handle the response
    If context.Node.OkResponse = selectedResponse Then
        ContinueJob()
    Else
        CancelJob()
    End If

    ' Apply the response
    context.Node.RespondDialog(context, selectedResponse)

    Return OpcStatusCode.Good
End Function

' DOC
Dim outOfProcessableBounds = New OpcAcknowledgeableConditionNode(machineOne, "OutOfBoundsAlert")

' Once the node has been added to the address space:
' Define the condition as: Needs to be acknowledged
outOfProcessableBounds.ChangeIsAcked(Me.SystemContext, False)

' Define the condition as: Needs to be confirmed
outOfProcessableBounds.ChangeIsConfirmed(Me.SystemContext, False)

' DOC
outOfProcessableBounds.AcknowledgeCallback =
    Function(context, eventId, comment)
        Console.WriteLine("Acknowledged: {0}", comment)
        Return OpcStatusCode.Good
    End Function

' Acknowledge or confirm the condition on the server side.
outOfProcessableBounds.Acknowledge(Me.SystemContext, "Acknowledged by the server")
outOfProcessableBounds.Confirm(Me.SystemContext)

' DOC
' Suppress the alarm, e.g. while the machine is in maintenance.
overheating.ChangeIsSuppressed(Me.SystemContext, True)

' Shelve the alarm for 30 minutes.
overheating.ChangeShelving(Me.SystemContext, True, False, TimeSpan.FromMinutes(30))

' Unshelve the alarm.
overheating.ChangeShelving(Me.SystemContext, False)

' DOC
Dim temperatureLimit = New OpcExclusiveLevelAlarmNode(
        machineOne, "TemperatureLimit", OpcLimitAlarmStates.High Or OpcLimitAlarmStates.HighHigh)

temperatureLimit.HighLimit = 80      ' e.g. °C
temperatureLimit.HighHighLimit = 100 ' e.g. °C

' Once the node has been added to the address space:
temperatureLimit.ChangeIsActive(Me.SystemContext, True)
temperatureLimit.ChangeLimitState(Me.SystemContext, OpcLimitAlarmStates.High)
