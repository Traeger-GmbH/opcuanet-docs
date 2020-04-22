// DOC
client.SubscribeEvent(OpcObjectTypes.Server, HandleGlobalEvents);

// DOC
private static void HandleGlobalEvents(
        object sender,
        OpcEventReceivedEventArgs e)
{
    Console.WriteLine(e.Event.Message);
}

// DOC
private static void HandleGlobalEvents( _
        object sender, _
        OpcEventReceivedEventArgs e)
{
    var alarm = e.Event as OpcAlarmCondition;
 
    if (alarm != null)
        Console.WriteLine("Alarm: " + alarm.Message);
}

// DOC
// Define an attribute operand using the identifier of the type which defines the
// attribute / property including the name of the attribute / property to evaluate
// by the operand.
var severity = new OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity");
var conditionName = new OpcSimpleAttributeOperand(OpcEventTypes.Condition, "ConditionName");
 
var filter = OpcFilter.Using(client)
        .FromEvents(OpcEventTypes.AlarmCondition)
        .Where(severity > OpcEventSeverity.Medium & conditionName.Like("Temperature"))
        .Select();
 
client.SubscribeEvent(
        OpcObjectTypes.Server,
        filter,
        HandleGlobalEvents);

// DOC
var severity = new OpcSimpleAttributeOperand(OpcEventTypes.Event, "Severity");
var conditionName = new OpcSimpleAttributeOperand(OpcEventTypes.Condition, "ConditionName");
 
var filter = OpcFilter.Using(client)
        .FromEvents(
            OpcEventTypes.AlarmCondition,
            OpcEventTypes.ExclusiveLimitAlarm,
            OpcEventTypes.DialogCondition)
        .Where(severity > OpcEventSeverity.Medium & conditionName.Like("Temperature"))
        .Select();
 
client.SubscribeEvent(
        OpcObjectTypes.Server,
        filter,
        HandleGlobalEvents);

// DOC
client.SubscribeEvent(machineNodeId, HandleLocalEvents);

// DOC
private static void HandleLocalEvents(
        object sender,
        OpcEventReceivedEventArgs e)
{
    Console.WriteLine(e.Event.Message);
}

// DOC
var subscription = client.SubscribeEvent(
        machineNodeId,
        HandleLocalEvents);
 
// Query most recent event information.
subscription.RefreshConditions();

// DOC
private static void HandleLocalEvents(
        object sender,
        OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcCondition;
 
    if (condition.IsRetained) {
        Console.Write((condition.ClientUserId ?? "Comment") + ":");
        Console.WriteLine(condition.Comment);
    }
}

// DOC
private static void HandleLocalEvents(object sender, OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcCondition;
 
    if (condition.IsEnabled && condition.Severity < OpcEventSeverity.Medium)
        condition.Disable(client);
}



// DOC
private static void HandleLocalEvents(object sender, OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcCondition;
 
    if (condition != null)
        condition.AddComment(client, "Evaluated by me!");
}



// DOC
private static void HandleLocalEvents(object sender, OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcDialogCondition;
 
    if (condition != null && condition.IsActive) {
        Console.WriteLine(condition.Prompt);
        Console.WriteLine("    Options:");
 
        var responseOptions = condition.ResponseOptions;
 
        for (int index = 0; index < responseOptions.Length; index++) {
            Console.Write($"      [{index}] = {responseOptions[index].Value}");
 
            if (index == condition.DefaultResponse)
                Console.Write(" (default)");
 
            Console.WriteLine();
        }
 
        var respond = string.Empty;
        var respondOption = condition.DefaultResponse;
 
        do {
            Console.Write("Enter the number of the option and press Enter to respond: ");
            respond = Console.ReadLine();
 
            if (string.IsNullOrEmpty(respond))
                break;
        } while (!int.TryParse(respond, out respondOption));
 
        condition.Respond(client, respondOption);
    }
}


// DOC
client.DialogRequested += HandleDialogRequested;
...
 
private static void HandleDialogRequested(
        object sender,
        OpcDialogRequestedEventArgs e)
{
    // Just use the default response, here.
    e.SelectedResponse = e.Dialog.DefaultResponse;
}

// DOC
private static void HandleLocalEvents(object sender, OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcAcknowledgeableCondition;
 
    if (condition != null && !condition.IsAcked) {
        Console.WriteLine($"Acknowledgment is required for condtion: {condition.ConditionName}");
        Console.WriteLine($"  -> {condition.Message}");
        Console.Write("Enter your acknowlegment comment and press Enter to acknowledge: ");
 
        var comment = Console.ReadLine();
        condition.Acknowledge(client, comment);
    }
}


// DOC
private static void HandleLocalEvents(
        object sender,
        OpcEventReceivedEventArgs e)
{
    var alarm = e.Event as OpcAlarmCondition;
 
    if (alarm != null) {
        Console.Write($"Alarm {alarm.ConditionName} is");
        Console.WriteLine($"{(alarm.IsActive ? "active" : "inactive")}!");
    }
}

// DOC
private static void HandleLocalEvents(object sender, OpcEventReceivedEventArgs e)
{
    var alarm = e.Event as OpcDiscreteAlarm;
 
    if (alarm != null) {
        if (alarm is OpcTripAlarm)
            Console.WriteLine("Trip Alarm!");
        else if (alarm is OpcOffNormalAlarm)
            Console.WriteLine("Off Normal Alarm!");
    }
}



// DOC
private static void HandleLocalEvents(
        object sender,
        OpcEventReceivedEventArgs e)
{
    var alarm = e.Event as OpcLimitAlarm;
 
    if (alarm != null) {
        Console.Write(alarm.LowLowLimit);
        Console.Write(" ≤ ");
        Console.Write(alarm.LowLimit);
        Console.Write(" ≤ ");
        Console.Write(alarm.HighLimit);
        Console.Write(" ≤ ");
        Console.Write(alarm.HighHighLimit);
    }
}
