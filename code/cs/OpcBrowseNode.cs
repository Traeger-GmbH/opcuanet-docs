// DOC
var node = client.BrowseNode(OpcObjectTypes.ObjectsFolder);
Browse(node);
...
private void Browse(OpcNodeInfo node, int level = 0)
{
    Console.WriteLine("{0}{1}({2})",
            new string('.', level * 4),
            node.Attribute(OpcAttribute.DisplayName).Value,
            node.NodeId);
 
    level++;
 
    foreach (var childNode in node.Children())
        Browse(childNode, level);
}

// DOC
OpcNodeInfo machineNode = client.BrowseNode("ns=2;s=Machine");

// DOC
OpcNodeInfo jobNode = machineNode.Child("Job");
 
foreach (var childNode in machineNode.Children()) {
    // Your code to operate on each child node.
}

// DOC
OpcAttributeInfo displayName = machineNode.Attribute(OpcAttribute.DisplayName);
 
foreach (var attribute in machineNode.Attributes()) {
    // Your code to operate on each attribute.
}

// DOC
if (childNode.Category == OpcNodeCategory.Method) {
    var methodNode = (OpcMethodNodeInfo)childNode;
 
    foreach (var argument in methodNode.GetInputArguments()) {
        // Your code to operate on each argument.
    }
}

// DOC
// Create a browse command to browse all hierarchical references.
var browse = new OpcBrowseNode(
        nodeId: OpcNodeId.Parse("ns=2;s=Machine"),
        degree: OpcBrowseNodeDegree.Generation);

// Create a browse command to browse specific types of references.
var browse = new OpcBrowseNode(
        nodeId: OpcNodeId.Parse("ns=2;s=Machine"),
        degree: OpcBrowseNodeDegree.Generation,
        referenceTypes: new[] {
            OpcReferenceType.Organizes,
            OpcReferenceType.HasComponent,
            OpcReferenceType.HasProperty
        });

// Reduce browsing to the smallest possible amount of data.
browse.Options = OpcBrowseOptions.IncludeReferenceTypeId
        | OpcBrowseOptions.IncludeBrowseName;

var node = client.BrowseNode(browse);

foreach (var childNode in node.Children()) {
    // Continue recursively...
}
