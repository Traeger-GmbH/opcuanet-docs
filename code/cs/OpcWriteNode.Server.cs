// DOC
var machineIsRunningNode = new OpcDataVariableNode<bool>("IsRunning", false);

// DOC
machineIsRunningNode.Value = true;

// DOC
machineIsRunningNode.Description = "My description";

// DOC
machineIsRunningNode.ApplyChanges(server.SystemContext);

// DOC
machineIsRunningNode.WriteDescriptionCallback = HandleWriteDescription;
...
private OpcAttributeValue<string> HandleWriteDescription(
        OpcWriteAttributeValueContext context,
        OpcAttributeValue<string> value)
{
    return WriteDescriptionToDataSource(context.Node, value) ?? value;
}

// DOC
machineIsRunningNode.WriteVariableValueCallback = HandleWriteVariableValue;
...
private OpcVariableValue<object> HandleWriteVariableValue(
        OpcWriteVariableValueContext context,
        OpcVariableValue<object> value)
{
    return WriteValueToDataSource(context.Node, value) ?? value;
}
