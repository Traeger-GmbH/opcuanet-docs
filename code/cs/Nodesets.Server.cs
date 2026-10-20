// DOC
var umatiManager = OpcNodeSetManager.Create(
        OpcNodeSet.Load(@".\umati.xml"),
        OpcNodeSet.Load(@".\umati-instances.xml"));

using (var server = new OpcServer("opc.tcp://localhost:4840/", umatiManager)) {
    server.Start();
    ...
}

// DOC
protected override IEnumerable<OpcNodeSet> ImportNodes()
{
    yield return OpcNodeSet.Load(@".\umati.xml");
    yield return OpcNodeSet.Load(@".\umati-instances.xml");
}

// DOC
protected override void ImplementNode(IOpcNode node)
{
    // Implement your Node(s) here.
}

// DOC
private static readonly OpcNodeId LampTypeId = "ns=2;i=1041";
private readonly Random random = new Random();

protected override void ImplementNode(IOpcNode node)
{
    if (node is OpcVariableNode variableNode && variableNode.Name == "2:Status") {
        if (variableNode?.Parent is OpcObjectNode objectNode && objectNode.TypeDefinitionId == LampTypeId) {
            variableNode.ReadVariableValueCallback = (context, value) => new OpcVariableValue<object>(this.random.Next(0, 2));
        }
    }
}


// DOC
var lampTypeId = OpcNodeId.Parse("ns=2;i=1041");
var random = new Random();

var umatiManager = OpcNodeSetManager.Create(
        OpcNodeSet.Load(@".\umati.xml"),
        OpcNodeSet.Load(@".\umati-instances.xml"));

umatiManager.ImplementNodeCallback = (context, node) => {
    if (node is OpcVariableNode variableNode && variableNode.Name == "2:Status") {
        if (variableNode.Parent is OpcObjectNode objectNode && objectNode.TypeDefinitionId == lampTypeId)
            variableNode.ReadVariableValueCallback = (readContext, value) => new OpcVariableValue<object>(random.Next(0, 2));
    }
};

// DOC
protected override void ImplementNode(IOpcNode node)
{
    // Keeps the default implementation, e.g. the references to the interfaces of the node.
    base.ImplementNode(node);

    // Implement your Node(s) here.
}

// DOC
umatiManager.ImportOptions = OpcNodeSetImportOptions.CompleteDataTypeDefinitionFields
        | OpcNodeSetImportOptions.CreateNonGenericNodeInstances;

// DOC
using (var stream = File.OpenRead(@".\umati.xml")) {
    var nodeSet = OpcNodeSet.Load(stream);

    foreach (var node in nodeSet.GetNodes())
        Console.WriteLine("{0}: {1}", node.Id, node.DisplayName);
}
