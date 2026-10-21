// DOC
OpcNodeId speedId = client.TranslatePath("/Objects/2:Machine/2:Speed");

if (speedId == null)
    Console.WriteLine("The path does not exist.");
else
    Console.WriteLine("Speed: {0}", client.ReadNode(speedId));

// DOC
OpcNodeId temperatureId = client.TranslatePath(
        OpcObjectTypes.ObjectsFolder,
        "2:Machine",
        "2:Temperature");

// DOC
OpcTranslatePathResultCollection results = client.TranslatePaths(
        new OpcTranslatePath("/Objects/2:Machine/2:Speed"),
        new OpcTranslatePath("/Objects/2:Machine/2:Temperature"),
        new OpcTranslatePath("/Objects/2:Machine/2:IsRunning"));

foreach (OpcTranslatePathResult result in results) {
    if (result.IsGood)
        Console.WriteLine(result.NodeId);
    else
        Console.WriteLine("Not found: {0}", result.Code);
}

// DOC
OpcValue speed = client.ReadNode("nsu=http://example.com/machine/;s=Machine/Speed");

// DOC
int index = client.Namespaces.ResolveIndex("http://example.com/machine/");

if (index >= 0) {
    var temperatureId = new OpcNodeId("Machine/Temperature", index);
    OpcValue temperature = client.ReadNode(temperatureId);
}

// DOC
OpcNodeIdCollection registeredNodeIds = client.RegisterNodes(
        "ns=2;s=Machine/Speed",
        "ns=2;s=Machine/Temperature");

OpcReadNode[] commands = registeredNodeIds
        .Select(nodeId => new OpcReadNode(nodeId))
        .ToArray();

for (int cycle = 0; cycle < 10; cycle++)
    client.ReadNodes(commands);

client.UnregisterNodes(registeredNodeIds);

// DOC
using (var discovery = new OpcDiscoveryClient("opc.tcp://localhost:4840/")) {
    foreach (OpcApplicationDescription server in discovery.DiscoverServers()) {
        Console.WriteLine("{0} ({1})", server.Name, server.Type);

        foreach (Uri url in server.DiscoveryUrls)
            Console.WriteLine("  {0}", url);
    }
}

// DOC
OpcNodeInfo machine = client.BrowseNode("ns=2;s=Machine");

OpcNodeInfo firstAlarm = Descendants(machine)
        .Where(node => node.Category == OpcNodeCategory.Variable)
        .FirstOrDefault(node => node.Name.Value.StartsWith("Alarm"));
...
private static IEnumerable<OpcNodeInfo> Descendants(OpcNodeInfo node)
{
    foreach (OpcNodeInfo child in node.Children()) {
        yield return child;

        foreach (OpcNodeInfo descendant in Descendants(child))
            yield return descendant;
    }
}

// DOC
var speed = new OpcNodeValue<double>("ns=2;s=Machine/Speed");

double current = speed.Read(client);

if (speed.Status.IsGood) {
    OpcStatus result = speed.Write(client, current + 10);
}

// DOC
var temperatures = new[] {
    new OpcNodeValue<double>("ns=2;s=Machine/Temperature1"),
    new OpcNodeValue<double>("ns=2;s=Machine/Temperature2"),
    new OpcNodeValue<double>("ns=2;s=Machine/Temperature3")
};

double[] values = temperatures.Read(client);

// DOC
using (var source = new OpcNodeStream(client, "ns=2;s=Machine/Snapshot"))
using (var target = File.Create("snapshot.bin")) {
    source.CopyTo(target);
}

// DOC
var grid = new double[2, 3] { { 1, 2, 3 }, { 4, 5, 6 } };
client.WriteNode("ns=2;s=Machine/Grid", grid);

double[,] values = (double[,])client.ReadNode("ns=2;s=Machine/Grid").Value;
double value = values[1, 2];

// DOC
var nodeIds = new OpcNodeId[] { "ns=2;s=Machine/Speed", "ns=2;s=Machine/Temperature" };

using (var history = client.ReadNodesHistory(
        DateTime.UtcNow.AddHours(-1),
        DateTime.UtcNow,
        100,
        nodeIds)) {
    foreach (OpcHistoryValue value in history[nodeIds[0]])
        Console.WriteLine("{0}: {1}", value.Timestamp, value.Value);
}

// DOC
IDictionary<OpcNodeId, IEnumerable<OpcHistoryValue>> history = client.ReadNodesHistory(
        DateTime.UtcNow.AddHours(-1),
        DateTime.UtcNow,
        "ns=2;s=Machine/Speed",
        "ns=2;s=Machine/Temperature");

foreach (var entry in history)
    Console.WriteLine("{0}: {1} values", entry.Key, entry.Value.Count());

// DOC
client.Locales.Add("de-DE");
client.Locales.Add("en-US");

client.Connect();

OpcValue displayName = client.ReadNode("ns=2;s=Machine", OpcAttribute.DisplayName);
