Dim isActive = New OpcSimpleAttributeOperand( _
        OpcEventTypes.AlarmCondition, _
        "ActiveState", "Id")
Dim isAcked = New OpcSimpleAttributeOperand( _
        OpcEventTypes.AcknowledgeableCondition, _
        "AckedState", "Id")

Dim filter = OpcFilter.Using(client) _
    .FromEvents(OpcEventTypes.AlarmCondition) _
    .Where(OpcFilterOperand.OfType(OpcEventTypes.AlarmCondition) _
        And isAcked = False _
        And isActive = True) _
    .Select()

Dim subscription = client.SubscribeEvent("ns=2;s=Machine", filter, _
    Sub(sender, e)
        Dim alarm = TryCast(e.Event, OpcAlarmCondition)

        If alarm IsNot Nothing Then
            Console.WriteLine( _
                    "{0}: {1}, IsActive = {2}, IsAcked = {3} ({4})", _
                    alarm.GetType().Name, _
                    alarm.Message, _
                    alarm.IsActive, _
                    alarm.IsAcked, _
                    alarm.Severity)
        End If
    End Sub)
