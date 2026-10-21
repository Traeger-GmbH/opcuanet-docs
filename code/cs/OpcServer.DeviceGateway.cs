// DOC
public class PlcNodeManager : OpcNodeManager
{
    private OpcDataVariableNode<double> temperatureNode;
    private OpcDataVariableNode<int> speedNode;

    public PlcNodeManager()
        : base("http://example.com/plc")
    {
    }

    protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
    {
        var machine = new OpcObjectNode(new OpcName("Machine", this.DefaultNamespaceIndex));
        references.Add(machine, OpcObjectTypes.ObjectsFolder);

        this.temperatureNode = new OpcDataVariableNode<double>(machine, "Temperature");
        this.speedNode = new OpcDataVariableNode<int>(machine, "Speed");

        // Clients may write the speed; the value is forwarded to the device.
        this.speedNode.AccessLevel = OpcAccessLevel.CurrentReadOrWrite;
        this.speedNode.UserAccessLevel = OpcAccessLevel.CurrentReadOrWrite;
        this.speedNode.WriteVariableValueCallback = this.HandleWriteSpeed;

        yield return machine;
    }
}

// DOC
public void Update(PlcReading reading)
{
    if (reading.IsAvailable) {
        this.temperatureNode.Value = reading.Temperature;
        this.temperatureNode.Status.Update(OpcStatusCode.Good);
        this.temperatureNode.Timestamp = reading.Timestamp;
    }
    else {
        // Keep the last value, but report that it is no longer up to date.
        this.temperatureNode.Status.Update(OpcStatusCode.BadCommunicationError);
        this.temperatureNode.Timestamp = DateTime.UtcNow;
    }

    this.temperatureNode.ApplyChanges(this.SystemContext);
}

// DOC
using (var plcManager = new PlcNodeManager())
using (var server = new OpcServer("opc.tcp://localhost:4840/", plcManager)) {
    server.Start();

    while (!cancellation.IsCancellationRequested) {
        plcManager.Update(Plc.ReadTemperature());
        Thread.Sleep(1000);
    }
}

// DOC
private OpcVariableValue<object> HandleWriteSpeed(
        OpcWriteVariableValueContext context,
        OpcVariableValue<object> value)
{
    if (!Plc.TryWriteSpeed((int)value.Value)) {
        // The client receives this status as the result of its write request.
        return new OpcVariableValue<object>(
                value.Value,
                DateTime.UtcNow,
                OpcStatusCode.BadCommunicationError);
    }

    return value;
}

// DOC
public void Poll()
{
    // Only query values a client has subscribed to (see MachineNodeManager).
    if (this.IsMonitored(this.temperatureNode.Id))
        this.Update(Plc.ReadTemperature());
}
