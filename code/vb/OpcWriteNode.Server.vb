'DOC
Dim machineIsRunningNode = New OpcDataVariableNode(Of Boolean)("IsRunning", False)

'DOC
machineIsRunningNode.Value = True

'DOC
machineIsRunningNode.Description = "My description"

'DOC
machineIsRunningNode.ApplyChanges(server.SystemContext)

'DOC
machineIsRunningNode.WriteDescriptionCallback = AddressOf HandleWriteDescription
...
Private Function HandleWriteDescription( _
        ByVal context As OpcWriteAttributeValueContext, _
        ByVal value As OpcAttributeValue(Of String)) As OpcAttributeValue(Of String)
    Return If(WriteDescriptionToDataSource(context.Node, value), value)
End Function


'DOC
machineIsRunningNode.WriteVariableValueCallback = AddressOf HandleWriteVariableValue
...
Private Function HandleWriteVariableValue( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    Return If(WriteValueToDataSource(context.Node, value), value)
End Function

