// DOC
var server = new OpcServer(...);
// ...

server.ReportEvent(
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation.");

// Same usage as before + arguments support.
server.ReportEvent(
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation at machine {0}.",
        machineId);

// Sames usage as before + source node.
server.ReportEvent(
        sourceNode,
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation.");

// Same usage as before + arguments support.
server.ReportEvent(
        sourceNode,
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation at machine {0}.",
        machineId);

// Same usage as before + explicit source information.
server.ReportEvent(
        sourceNodeId,
        sourceNodeName,
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation.");

// Same usage as before + arguments support.
server.ReportEvent(
        sourceNodeId,
        sourceNodeName,
        OpcEventSeverity.Medium,
        "Recognized a medium urgent situation at machine {0}.",
        machineId);
		
// DOC
var activatedEvent = new OpcEventNode(machineOne, "Activated");

// DOC
machineOne.AddNotifier(this.SystemContext, activatedEvent);

// DOC
activatedEvent.SourceNodeId = sourceNodeId;
activatedEvent.SourceName = sourceNodeName;
activatedEvent.Severity = OpcEventSeverity.Medium;
activatedEvent.Message = "Recognized a medium urgent situation.";

// DOC
// Server generated value to identify a specific Event
activatedEvent.EventId = ...;

// The time the event occured
activatedEvent.Time = ...;

// The time the event has been received by the underlaying system / device
activatedEvent.ReceiveTime = ...;

// DOC
activatedEvent.ReportEvent(this.SystemContext);

// DOC
machineOne.QueryEventsCallback = (context, events) => {
    // Ensure that an re-entrance upon notifier cross-references will not add
    // events to the collection which are already stored in.
    if (events.Count != 0)
        return;

    events.Add(activatedEvent.CreateEvent(context));
};

// DOC
var maintenanceEvent = new OpcConditionNode(machineOne, "Maintenance");

// Interesting for a client yes or no
maintenanceEvent.IsRetained = true; // = default

// Condition is enabled or disabled
maintenanceEvent.IsEnabled; // use ChangeIsEnabled(...)

// Status of the source the condition is based upon
maintenanceEvent.Quality = ...;

// DOC
// Identifier of the user who supplied the Comment
maintenanceEvent.ClientUserId = ...;

// Last comment provided by a user
maintenanceEvent.Comment = ...;

// DOC 
// Uses a new GUID as BranchId
var maintenanceBranchA = maintenanceEvent.CreateBranch(this.SystemContext);

// Uses a custom NodeId as BranchId
var maintenanceBranchB = maintenanceEvent.CreateBranch(this.SystemContext, new OpcNodeId(10001));

...

// Identifies the branch of the event
maintenanceEvent.BranchId = ...;

// Previous severity of the branch
maintenanceEvent.LastSeverity = ...;

// DOC
var outOfMaterial = new OpcDialogConditionNode(machineOne, "MaterialAlert");

outOfMaterial.Message = "Out of Material"; // Generic event message
outOfMaterial.Prompt = "The machine is out of material. Refill material supply to continue.";
outOfMaterial.ResponseOptions = new OpcText[] { "Continue", "Cancel" };
outOfMaterial.DefaultResponse = 0; // Index of ResponseOption to use
outOfMaterial.CancelResponse = 1;  // Index of ResponseOption to use
outOfMaterial.OkResponse = 0;      // Index of ResponseOption to use

// DOC
outOfMaterial.RespondCallback = this.HandleOutOfMaterialResponse;

...

private OpcStatusCode HandleOutOfMaterialResponse(
        OpcNodeContext<OpcDialogConditionNode> context,
        int selectedResponse)
{
    // Handle the response
    if (context.Node.OkResponse == selectedResponse)
        ContinueJob();
    else
        CancelJob();

    // Apply the response
    context.Node.RespondDialog(context, response);

    return OpcStatusCode.Good;
}

// DOC
var outOfProcessableBounds = new OpcAcknowledgeableConditionNode(machineOne, "OutOfBoundsAlert");

// Define the condition as: Needs to be acknowledged
outOfProcessableBounds.ChangeIsAcked(this.SystemContext, false);

// Define the condition as: Needs to be confirmed
outOfProcessableBounds.ChangeIsConfirmed(this.SystemContext, false);

// DOC
if (outOfProcessableBounds.IsAcked) {
    ...
}

if (outOfProcessableBounds.IsConfirmed) {
    ...
}

// DOC
var overheating = new OpcAlarmConditionNode(machineOne, "OverheatingAlert");
var idle = new OpcAlarmConditionNode(machineOne, "IdleAlert");

...

overheating.ChangeIsActive(this.SystemContext, true);
idle.ChangeIsActive(this.SystemContext, true);

...

if (overheating.IsActive)
    CancelJob();

if (!idle.IsActive)
    ProcessJob();
else if (idle.IsSuppressed)
    SimulateJob();

// DOC
var x = new OpcDiscreteAlarmNode(machineOne, "discreteAlert");
var y = new OpcOffNormalAlarmNode(machineOne, "offNormalAlert");
var z = new OpcTripAlarmNode(machineOne, "tripAlert");

// DOC
var positionLimit = new OpcLimitAlarmNode(
        machineOne, "PositionLimit", OpcLimitAlarmStates.HighHigh | OpcLimitAlarmStates.LowLow);

positionLimit.HighHighLimit = 120; // e.g. mm
positionLimit.LowLowLimit = 0;     // e.g. mm

// DOC
// Applies the changes of the event node and sends the event to all subscribers.
server.ReportEvent(activatedEvent);

// DOC
var maintenanceEvent = new OpcConditionNode(machineOne, "Maintenance");

// Default: EnabledOnly for an OpcConditionNode,
// ActiveOrUnacknowledged for acknowledgeable conditions and alarms.
maintenanceEvent.IsRetainedPolicy = OpcConditionRetainPolicy.Manual;

// Interesting for a client yes or no (only kept in case of the Manual policy)
maintenanceEvent.IsRetained = true;

// Status of the source the condition is based upon
maintenanceEvent.Quality = OpcStatusCode.Good;

// DOC
// Use the Change methods once the node has been added to the address space.
maintenanceEvent.ChangeIsEnabled(this.SystemContext, false);
maintenanceEvent.ChangeComment(this.SystemContext, "Planned maintenance");

// Reports an event whenever one of the Change methods completes.
maintenanceEvent.AutoReportChanges = true;

// DOC
// Uses a new GUID as BranchId
var maintenanceBranchA = maintenanceEvent.CreateBranch(this.SystemContext);

// Uses a custom NodeId as BranchId
var maintenanceBranchB = maintenanceEvent.CreateBranch(this.SystemContext, new OpcNodeId(10001));

// DOC
outOfMaterial.RespondCallback = this.HandleOutOfMaterialResponse;
outOfMaterial.ActivateDialog(this.SystemContext);
...
private OpcStatusCode HandleOutOfMaterialResponse(
        OpcNodeContext<OpcDialogConditionNode> context,
        int selectedResponse)
{
    // Handle the response
    if (context.Node.OkResponse == selectedResponse)
        ContinueJob();
    else
        CancelJob();

    // Apply the response
    context.Node.RespondDialog(context, selectedResponse);

    return OpcStatusCode.Good;
}

// DOC
var outOfProcessableBounds = new OpcAcknowledgeableConditionNode(machineOne, "OutOfBoundsAlert");

// Once the node has been added to the address space:
// Define the condition as: Needs to be acknowledged
outOfProcessableBounds.ChangeIsAcked(this.SystemContext, false);

// Define the condition as: Needs to be confirmed
outOfProcessableBounds.ChangeIsConfirmed(this.SystemContext, false);

// DOC
outOfProcessableBounds.AcknowledgeCallback = (context, eventId, comment) => {
    Console.WriteLine("Acknowledged: {0}", comment);
    return OpcStatusCode.Good;
};

// Acknowledge or confirm the condition on the server side.
outOfProcessableBounds.Acknowledge(this.SystemContext, "Acknowledged by the server");
outOfProcessableBounds.Confirm(this.SystemContext);

// DOC
// Suppress the alarm, e.g. while the machine is in maintenance.
overheating.ChangeIsSuppressed(this.SystemContext, true);

// Shelve the alarm for 30 minutes.
overheating.ChangeShelving(this.SystemContext, true, false, TimeSpan.FromMinutes(30));

// Unshelve the alarm.
overheating.ChangeShelving(this.SystemContext, false);

// DOC
var temperatureLimit = new OpcExclusiveLevelAlarmNode(
        machineOne, "TemperatureLimit", OpcLimitAlarmStates.High | OpcLimitAlarmStates.HighHigh);

temperatureLimit.HighLimit = 80;      // e.g. °C
temperatureLimit.HighHighLimit = 100; // e.g. °C

// Once the node has been added to the address space:
temperatureLimit.ChangeIsActive(this.SystemContext, true);
temperatureLimit.ChangeLimitState(this.SystemContext, OpcLimitAlarmStates.High);
