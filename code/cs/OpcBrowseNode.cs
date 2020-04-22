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
