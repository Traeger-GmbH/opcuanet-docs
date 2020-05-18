'DOC
client.SubscribeEvent(OpcObjectTypes.Server, AddressOf HandleGlobalEvents)

'DOC
Private Shared Sub HandleGlobalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Console.WriteLine(e.Event.Message)
End Sub


'DOC
Private Shared Sub HandleGlobalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim alarm = TryCast(e.Event, OpcAlarmCondition)

    If alarm IsNot Nothing Then
        Console.WriteLine("Alarm: " & alarm.Message)
    End If
End Sub

'DOC
'Define an attribute operand using the identifier of the type which defines the
'attribute / property including the name of the attribute / property to evaluate
'by the operand.
Dim severity = New OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity")
Dim conditionName = New OpcSimpleAttributeOperand(OpcEventTypes.Condition, "ConditionName")

Dim filter = OpcFilter.Using(client) _
        .FromEvents(OpcEventTypes.AlarmCondition) _
        .Where(severity > OpcEventSeverity.Medium And conditionName.Like("Temperature")) _
        .Select()

client.SubscribeEvent(
        OpcObjectTypes.Server,
        filter,
        AddressOf HandleGlobalEvents)

'DOC
Dim severity = New OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity")
Dim conditionName = New OpcSimpleAttributeOperand(OpcEventTypes.Condition, "ConditionName")

Dim filter = OpcFilter.Using(client) _
        .FromEvents( _
            OpcEventTypes.AlarmCondition, _
            OpcEventTypes.ExclusiveLimitAlarm, _
            OpcEventTypes.DialogCondition) _
        .Where(severity > OpcEventSeverity.Medium And conditionName.Like("Temperature")) _
        .Select()

client.SubscribeEvent(
        OpcObjectTypes.Server,
        filter,
        AddressOf HandleGlobalEvents)

'DOC
client.SubscribeEvent(machineNodeId, AddressOf HandleLocalEvents)

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Console.WriteLine(e.Event.Message)
End Sub


'DOC
Dim subscription = client.SubscribeEvent( _
        machineNodeId, _
        AddressOf HandleLocalEvents)
 
'Query most recent event information.
subscription.RefreshConditions()

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcCondition)

    If condition.IsRetained Then
        Console.Write((If(condition.ClientUserId, "Comment")) & ":")
        Console.WriteLine(condition.Comment)
    End If
End Sub


'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcCondition)

    If condition.IsEnabled AndAlso condition.Severity < OpcEventSeverity.Medium Then
        condition.Disable(client)
    End If
End Sub

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcCondition)

    If condition IsNot Nothing Then
        condition.AddComment(client, "Evaluated by me!")
    End If
End Sub

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcDialogCondition)

    If condition IsNot Nothing AndAlso condition.IsActive Then
        Console.WriteLine(condition.Prompt)
        Console.WriteLine("    Options:")

        Dim responseOptions = condition.ResponseOptions

        For index As Integer = 0 To responseOptions.Length - 1
            Console.Write("      [{0}] = {1}", index, responseOptions(index).Value)

            If index = condition.DefaultResponse Then _
                Console.Write(" (default)")

            Console.WriteLine()
        Next

        Dim respond = String.Empty
        Dim respondOption = condition.DefaultResponse

        Do
            Console.Write("Enter the number of the option and press Enter to respond: ")
            respond = Console.ReadLine()

            If String.IsNullOrEmpty(respond) _
                Then Exit Do
        Loop While Not Integer.TryParse(respond, respondOption)

        condition.Respond(client, respondOption)
    End If
End Sub

'DOC
AddHandler(client.DialogRequested, AddressOf HandleDialogRequested);
...
 
Private Shared Sub HandleDialogRequested( _
        ByVal sender As Object, _
        ByVal e As OpcDialogRequestedEventArgs)
    'Just use the default response, here.
    e.SelectedResponse = e.Dialog.DefaultResponse
End Sub


'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim condition = TryCast(e.Event, OpcAcknowledgeableCondition)

    If condition IsNot Nothing AndAlso Not condition.IsAcked Then
        Console.WriteLine("Acknowledgment is required for condtion: {0}", condition.ConditionName)
        Console.WriteLine("  -> {0}", condition.Message)
        Console.Write("Enter your acknowlegment comment and press Enter to acknowledge: ")

        Dim comment = Console.ReadLine()
        condition.Acknowledge(client, comment)
    End If
End Sub

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim alarm = TryCast(e.Event, OpcAlarmCondition)

    If alarm IsNot Nothing Then
        Console.Write("Alarm {0} is", alarm.ConditionName)
        Console.WriteLine("{0}!", If(alarm.IsActive, "active", "inactive"))
    End If
End Sub


'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim alarm = TryCast(e.Event, OpcDiscreteAlarm)

    If alarm IsNot Nothing Then
        If TypeOf alarm Is OpcTripAlarm Then
            Console.WriteLine("Trip Alarm!")
        ElseIf TypeOf alarm Is OpcOffNormalAlarm Then
            Console.WriteLine("Off Normal Alarm!")
        End If
    End If
End Sub

'DOC
Private Shared Sub HandleLocalEvents( _
        ByVal sender As Object, _
        ByVal e As OpcEventReceivedEventArgs)
    Dim alarm = TryCast(e.Event, OpcLimitAlarm)

    If alarm IsNot Nothing Then
        Console.Write(alarm.LowLowLimit)
        Console.Write(" ≤ ")
        Console.Write(alarm.LowLimit)
        Console.Write(" ≤ ")
        Console.Write(alarm.HighLimit)
        Console.Write(" ≤ ")
        Console.Write(alarm.HighHighLimit)
    End If
End Sub

