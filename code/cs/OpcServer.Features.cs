// DOC
DateTime endTime = DateTime.UtcNow;
DateTime startTime = endTime.AddHours(-8);

var averagePerHour = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Average,
        3600000,
        "ns=2;s=Machine/Temperature");

OpcHistoryValue atNoon = client.ReadNodeHistoryAtTime(
        DateTime.UtcNow.Date.AddHours(12),
        "ns=2;s=Machine/Temperature");

// DOC
public class MachineNodeManager : OpcNodeManager
{
    private readonly Dictionary<OpcNodeId, int> monitored = new Dictionary<OpcNodeId, int>();

    public MachineNodeManager()
        : base("http://example.com/machine")
    {
        this.MonitoredItemCreated += this.HandleMonitoredItemCreated;
        this.MonitoredItemDeleted += this.HandleMonitoredItemDeleted;
    }

    public bool IsMonitored(OpcNodeId nodeId)
    {
        lock (this.monitored)
            return this.monitored.ContainsKey(nodeId);
    }

    private void HandleMonitoredItemCreated(object sender, OpcMonitoredItemEventArgs e)
    {
        if (e.Item.Attribute != OpcAttribute.Value)
            return;

        lock (this.monitored) {
            this.monitored.TryGetValue(e.Item.NodeId, out int count);
            this.monitored[e.Item.NodeId] = count + 1;
        }
    }

    private void HandleMonitoredItemDeleted(object sender, OpcMonitoredItemEventArgs e)
    {
        if (e.Item.Attribute != OpcAttribute.Value)
            return;

        lock (this.monitored) {
            if (!this.monitored.TryGetValue(e.Item.NodeId, out int count))
                return;

            if (count > 1)
                this.monitored[e.Item.NodeId] = count - 1;
            else
                this.monitored.Remove(e.Item.NodeId);
        }
    }
}

// DOC
public class DeviceNodeManager : OpcNodeManager
{
    public DeviceNodeManager()
        : base("http://example.com/device")
    {
        this.BeforeRead += this.HandleBeforeRead;
        this.AfterWrite += this.HandleAfterWrite;
    }

    private void HandleBeforeRead(object sender, OpcNodeAccessEventArgs e)
    {
        OpcDataVariableNode[] nodes = e.Tokens
                .Where(token => token.Attribute == OpcAttribute.Value)
                .Select(token => token.Node)
                .OfType<OpcDataVariableNode>()
                .Distinct()
                .ToArray();

        // Device stands for the connection to the data source:
        // one request for all nodes of the client request.
        object[] values = Device.Read(nodes.Select(node => node.SymbolicName).ToArray());

        for (int index = 0; index < nodes.Length; index++)
            nodes[index].Value = values[index];
    }

    private void HandleAfterWrite(object sender, OpcNodeAccessEventArgs e)
    {
        var written = e.Tokens
                .Where(token => token.Attribute == OpcAttribute.Value && token.Result.IsGood)
                .ToArray();

        // One request to the device for all values written by the client.
        Device.Write(
                written.Select(token => token.Node.SymbolicName).ToArray(),
                written.Select(token => token.Value.Value).ToArray());
    }
}

// DOC
var temperature = new OpcDataVariableNode<double>("Temperature");
temperature.DisplayName = new OpcText("Temperature", "en-US", "Temperature.DisplayName");
temperature.Description = new OpcText("Temperature of the machine", "en-US", "Temperature.Description");

using (var server = new OpcServer("opc.tcp://localhost:4840/", temperature)) {
    server.Globalization.AddResource("de-DE", "Temperature.DisplayName", "Temperatur");
    server.Globalization.AddResource("de-DE", "Temperature.Description", "Temperatur der Maschine");

    server.Start();
}

// DOC
OpcAutomatism.RaiseModelChangeEvents = true;

// DOC
var machine = new OpcObjectNode(machines, "Machine3");
new OpcDataVariableNode<double>(machine, "Temperature");

manager.AddNode(server.SystemContext, machine);

// DOC
machines.SuspendModelChangeEvents(recursive: true);

foreach (string name in new[] { "Machine4", "Machine5", "Machine6" })
    manager.AddNode(server.SystemContext, new OpcObjectNode(machines, name));

machines.ResumeModelChangeEvents(recursive: true);

// DOC
var startJob = new OpcMethodNode(
        machine,
        "StartJob",
        new Func<string, Task<int>>(StartJobAsync));

// DOC
private static async Task<int> StartJobAsync(string jobName)
{
    // Long running work, e.g. waiting for the machine.
    await Task.Delay(TimeSpan.FromSeconds(5));
    return 42;
}

// DOC
public class JobNodeManager : OpcNodeManager
{
    public JobNodeManager()
        : base("http://example.com/jobs")
    {
    }

    protected override bool UseAsyncMethodCalls
    {
        get => true;
    }
}

// DOC
using (var server = new OpcServer("opc.tcp://localhost:4840/")) {
    OpcNamespace[] namespaces = server.RegisterNamespaces(
            "http://example.com/machine",
            "http://example.com/plant");

    Console.WriteLine("Machine: ns={0}", namespaces[0].Index);
    Console.WriteLine("Plant: ns={0}", namespaces[1].Index);

    server.Start();
}

// DOC
var profile = new OpcDataVariableNode<double[]>(machine, "Profile", new double[100]);
var heatMap = new OpcDataVariableNode<double[,]>(machine, "HeatMap", new double[16, 16]);

// DOC
double[,] values = heatMap.Value;
values[3, 4] = 72.5;

heatMap.Value = values;
heatMap.ApplyChanges(server.SystemContext);

// DOC
var tags = new[] {
    new { Name = "Temperature", Type = "double" },
    new { Name = "IsRunning", Type = "bool" },
    new { Name = "JobName", Type = "string" }
};

var server = new OpcServer("opc.tcp://localhost:4840/", space => {
    var machine = new OpcFolderNode("Machine");

    foreach (var tag in tags) {
        switch (tag.Type) {
            case "double":
                new OpcDataVariableNode<double>(machine, tag.Name);
                break;
            case "bool":
                new OpcDataVariableNode<bool>(machine, tag.Name);
                break;
            default:
                new OpcDataVariableNode<string>(machine, tag.Name);
                break;
        }
    }

    space.AddNode(machine);
});

// DOC
OpcServerInfo info = server.GetInfo();

Console.WriteLine("{0} {1} {2}", info.CompanyName, info.ProductName, info.ProductVersion);
Console.WriteLine("Product URI: {0}", info.ProductUri);

// DOC
[assembly: AssemblyCompany("Example Machines Ltd.")]
[assembly: AssemblyTitle("Example Machine Server")]
[assembly: AssemblyVersion("1.2.0.0")]

// DOC
public class MachineServer : OpcServer
{
    public MachineServer()
        : base("opc.tcp://localhost:4840/")
    {
    }

    protected override OpcServerInfo GetInfoCore()
    {
        return new OpcServerInfo(
                "Example Machines Ltd.",
                "Example Machine Server",
                new Version(1, 2, 0, 0),
                new DateTime(2026, 1, 15),
                new Uri("urn:example:machine-server"));
    }
}
