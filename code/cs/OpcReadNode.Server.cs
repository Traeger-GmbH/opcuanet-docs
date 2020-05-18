// DOC
var machineIsRunningNode = new OpcDataVariableNode<bool>("IsRunning", false);

// DOC
machineIsRunningNode.Value = true;

// DOC
machineIsRunningNode.Description = "My description";

// DOC
machineIsRunningNode.ApplyChanges(server.SystemContext);

// DOC
machineIsRunningNode.ReadDescriptionCallback = HandleReadDescription;
...
private OpcAttributeValue<string> HandleReadDescription(
        OpcReadAttributeValueContext context,
        OpcAttributeValue<string> value)
{
    return ReadDescriptionFromDataSource(context.Node) ?? value;
}

// DOC
machineIsRunningNode.ReadVariableValueCallback = HandleReadVariableValue;
...
private OpcVariableValue<object> HandleReadVariableValue(
        OpcReadVariableValueContext context,
        OpcVariableValue<object> value)
{
    return ReadValueFromDataSource(context.Node) ?? value;
}
