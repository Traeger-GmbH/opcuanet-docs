// DOC
// "this" points to the Node-Manager of the node.
var machineIsRunningHistorian = new OpcNodeHistorian(this, machineIsRunningNode);

// DOC
machineIsRunningNode.AccessLevel |= OpcAccessLevel.HistoryReadOrWrite;
machineIsRunningNode.UserAccessLevel |= OpcAccessLevel.HistoryReadOrWrite;
 
machineIsRunningNode.IsHistorizing = true;

// DOC
machineIsRunningHistorian.AutoUpdateHistory = true;

// DOC
machineIsRunningNode.BeforeApplyChanges += HandleBeforeApplyChanges;
...
private void HandleBeforeApplyChanges(object sender, EventArgs e)
{
    // Update (modified) Node History here.
}

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunnigNode)
        return machineIsRunningHistorian;
 
    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public IEnumerable<OpcHistoryValue> ReadHistory(
        OpcContext context,
        DateTime? startTime,
        DateTime? endTime,
        OpcReadHistoryOptions options)
{
    // Read (modified) Node History here.
}

// DOC
protected override IEnumerable<OpcHistoryValue> ReadHistory(
        IOpcNode node,
        DateTime? startTime,
        DateTime? endTime,
        OpcReadHistoryOptions options)
{
    // Read (modified) Node History here.
}

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunnigNode)
        return machineIsRunningHistorian;
 
    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public OpcStatusCollection CreateHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Create (modified) Node History here.
}

// DOC
protected override OpcStatusCollection CreateHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Create (modified) Node History here.
}

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunnigNode)
        return machineIsRunningHistorian;
 
    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public OpcStatusCollection DeleteHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        IEnumerable<DateTime> times)
{
    // Delete Node History entries and add them to the modified Node History here.
}
 
public OpcStatusCollection DeleteHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Delete Node History entries and add them to the modified Node History here.
}
 
public OpcStatusCollection DeleteHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        DateTime? startTime,
        DateTime? endTime,
        OpcDeleteHistoryOptions options)
{
    // Delete Node History entries and add them to the modified Node History here.
}

// DOC
protected override OpcStatusCollection DeleteHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        IEnumerable<DateTime> times)
{
    // Delete Node History entries and add them to the modified Node History here.
}
 
protected override OpcStatusCollection DeleteHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Delete Node History entries and add them to the modified Node History here.
}
 
protected override OpcStatusCollection DeleteHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        DateTime? startTime,
        DateTime? endTime,
        OpcDeleteHistoryOptions options)
{
    // Delete Node History entries and add them to the modified Node History here.
}

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunnigNode)
        return machineIsRunningHistorian;
 
    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public OpcStatusCollection ReplaceHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Replace Node History entries and add them to the modified Node History here.
}

// DOC
protected override OpcStatusCollection ReplaceHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Replace Node History entries and add them to the modified Node History here.
}

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunnigNode)
        return machineIsRunningHistorian;
 
    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public OpcStatusCollection UpdateHistory(
        OpcContext context,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Update (modified) Node History entries here.
}

// DOC
protected override OpcStatusCollection UpdateHistory(
        IOpcNode node,
        OpcHistoryModificationInfo modificationInfo,
        OpcValueCollection values)
{
    // Update (modified) Node History entries here.
}

// DOC
var history = new OpcNodeHistory<OpcHistoryValue>();

// DOC
var modifiedHistory = new OpcNodeHistory<OpcModifiedHistoryValue>();

// DOC
var results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count);
 
for (int index = 0; index < values.Count; index++) {
    var result = results[index];
    var value = OpcHistoryValue.Create(values[index]);
 
    if (MatchesValueType(value)) {
        if (history.Contains(value.Timestamp)) {
            result.Update(OpcStatusCode.BadEntryExists);
        }
        else {
            history.Add(value);
 
            var modifiedValue = value.CreateModified(modificationInfo);
            modifiedHistory.Add(modifiedValue);
 
            result.Update(OpcStatusCode.GoodEntryInserted);
        }
    }
    else {
        result.Update(OpcStatusCode.BadTypeMismatch);
    }
}
 
return results;

// DOC
var results = OpcStatusCollection.Create(OpcStatusCode.Good, times.Count());
 
int index = 0;
 
foreach (var time in times) {
    var result = results[index++];
 
    if (this.history.Contains(time)) {
        var value = this.history[time];
        this.history.RemoveAt(time);
 
        var modifiedValue = value.CreateModified(modificationInfo);
        this.modifiedHistory.Add(modifiedValue);
    }
    else {
        result.Update(OpcStatusCode.BadNoEntryExists);
    }
}
 
return results;

// DOC
var results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count);
 
for (int index = 0; index < values.Count; index++) {
    var timestamp = OpcHistoryValue.Create(values[index]).Timestamp;
    var result = results[index];
 
    if (history.Contains(timestamp)) {
        var value = history[timestamp];
        history.RemoveAt(timestamp);
 
        var modifiedValue = value.CreateModified(modificationInfo);
        modifiedHistory.Add(modifiedValue);
    }
    else {
        result.Update(OpcStatusCode.BadNoEntryExists);
    }
}
 
return results;

// DOC
var results = new OpcStatusCollection();
 
bool isModified = (options & OpcDeleteHistoryOptions.Modified)
        == OpcDeleteHistoryOptions.Modified;
 
if (isModified) {
    modifiedHistory.RemoveRange(startTime, endTime);
}
else {
    var values = history.Enumerate(startTime, endTime).ToArray();
    history.RemoveRange(startTime, endTime);
 
    for (int index = 0; index < values.Length; index++) {
        var value = values[index];
        modifiedHistory.Add(value.CreateModified(modificationInfo));
 
        results.Add(OpcStatusCode.Good);
    }
}
 
return results;

// DOC
var results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count);
 
for (int index = 0; index < values.Count; index++) {
    var result = results[index];
    var value = OpcHistoryValue.Create(values[index]);
 
    if (this.MatchesNodeValueType(value)) {
        if (this.history.Contains(value.Timestamp)) {
            var oldValue = this.history[value.Timestamp];
            history.Replace(value);
 
            var modifiedValue = oldValue.CreateModified(modificationInfo);
            modifiedHistory.Add(modifiedValue);
 
            result.Update(OpcStatusCode.GoodEntryReplaced);
        }
        else {
            result.Update(OpcStatusCode.BadNoEntryExists);
        }
    }
    else {
        result.Update(OpcStatusCode.BadTypeMismatch);
    }
}
 
return results;

// DOC
var results = OpcStatusCollection.Create(OpcStatusCode.Good, values.Count);
 
for (int index = 0; index < values.Count; index++) {
    var result = results[index];
    var value = OpcHistoryValue.Create(values[index]);
 
    if (MatchesValueType(value)) {
        if (history.Contains(value.Timestamp)) {
            var oldValue = this.history[value.Timestamp];
            history.Replace(value);
 
            var modifiedValue = oldValue.CreateModified(modificationInfo);
            modifiedHistory.Add(modifiedValue);
 
            result.Update(OpcStatusCode.GoodEntryReplaced);
        }
        else {
            history.Add(value);
 
            var modifiedValue = value.CreateModified(modificationInfo);
            modifiedHistory.Add(modifiedValue);
 
            result.Update(OpcStatusCode.GoodEntryInserted);
        }
    }
    else {
        result.Update(OpcStatusCode.BadTypeMismatch);
    }
}
 
return results;

// DOC
bool isModified = (options & OpcReadHistoryOptions.Modified)
        == OpcReadHistoryOptions.Modified;
 
if (isModified) {
    return modifiedHistory
            .Enumerate(startTime, endTime)
            .Cast<OpcHistoryValue>()
            .ToArray();
}
 
return history
        .Enumerate(startTime, endTime)
        .ToArray();

// DOC
protected override IOpcNodeHistoryProvider RetrieveNodeHistoryProvider(IOpcNode node)
{
    if (node == machineIsRunningNode)
        return machineIsRunningHistorian;

    return base.RetrieveNodeHistoryProvider(node);
}

// DOC
public IEnumerable<OpcHistoryValue> ReadHistory(
        OpcContext context,
        DateTime? startTime,
        DateTime? endTime,
        bool timeFlowsBackward,
        uint? maxCount,
        OpcReadHistoryOptions options)
{
    // Read (modified) Node History here.
}

// DOC
protected override IEnumerable<OpcHistoryValue> ReadHistory(
        OpcContext context,
        IOpcNode node,
        DateTime? startTime,
        DateTime? endTime,
        bool timeFlowsBackward,
        uint? maxCount,
        OpcReadHistoryOptions options)
{
    // Read (modified) Node History here.
}

// DOC
bool isModified = (options & OpcReadHistoryOptions.Modified)
        == OpcReadHistoryOptions.Modified;

OpcHistoryValue[] values;

if (isModified) {
    values = modifiedHistory
            .Enumerate(startTime, endTime)
            .Cast<OpcHistoryValue>()
            .ToArray();
}
else {
    values = history
            .Enumerate(startTime, endTime)
            .ToArray();
}

// Return the values in reverse order in case the time flows backward.
if (timeFlowsBackward)
    Array.Reverse(values);

return values;

// DOC
public class MachineEventHistory : IOpcEventHistoryProvider
{
    private readonly List<OpcEvent> events = new List<OpcEvent>();

    public MachineEventHistory(OpcObjectNode node)
    {
        this.Node = node;
        this.Node.IsHistorizingEvents = true;
    }

    public OpcObjectNode Node { get; }

    public void Add(OpcEvent historyEvent)
    {
        lock (this.events)
            this.events.Add(historyEvent);
    }

    public IEnumerable<OpcEvent> ReadEventHistory(
            OpcContext context,
            OpcEventFilter filter,
            DateTime? startTime,
            DateTime? endTime,
            bool timeFlowsBackward,
            uint? maxCount)
    {
        lock (this.events) {
            var from = timeFlowsBackward ? endTime : startTime;
            var to = timeFlowsBackward ? startTime : endTime;

            IEnumerable<OpcEvent> result = this.events.Where(item =>
                    (from == null || from == DateTime.MinValue || item.Time >= from)
                    && (to == null || to == DateTime.MinValue || item.Time <= to));

            if (timeFlowsBackward)
                result = result.Reverse();

            if (maxCount != null)
                result = result.Take((int)maxCount.Value);

            return result.ToArray();
        }
    }
}

// DOC
protected override IOpcEventHistoryProvider RetrieveEventHistoryProvider(IOpcNode node)
{
    if (node == this.machineEventHistory.Node)
        return this.machineEventHistory;

    return base.RetrieveEventHistoryProvider(node);
}

// DOC
activatedEvent.Message = "Machine started";
activatedEvent.ReportEvent(this.SystemContext);

// Stores a snapshot of the event in the event history.
this.machineEventHistory.Add(activatedEvent.CreateEvent(this.SystemContext));
