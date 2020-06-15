this.variableNode.WriteVariableValueCallback = this.WriteToNode;

// Deny write of a specific type of value.
private OpcVariableValue WriteToNode(
        OpcWriteVariableValueContext context,
        OpcVariableValue value)
{
    if (!(value.Value is short))
        value.Status.Update(OpcStatusCode.BadTypeMismatch);

    return value;
}

// Inline change type of value to the expected type.
private OpcVariableValue WriteToNode(
        OpcWriteVariableValueContext context,
        OpcVariableValue value)
{
    if (!(value.Value is short)) {
        value = new OpcVariableValue(
                Convert.ChangeType(value.Value, typeof(short)),
                value.Timestamp ?? DateTime.UtcNow,
                value.Status);
    }

    return value;
}
