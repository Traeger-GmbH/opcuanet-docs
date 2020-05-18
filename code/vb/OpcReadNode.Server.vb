'DOC
Dim machineIsRunningNode = New OpcDataVariableNode(Of Boolean)("IsRunning", False)

'DOC
machineIsRunningNode.Value = True

'DOC
machineIsRunningNode.Description = "My description"

'DOC
machineIsRunningNode.ApplyChanges(server.SystemContext)

'DOC
machineIsRunningNode.ReadDescriptionCallback = AddressOf HandleReadDescription
...
Private Function HandleReadDescription( _
        ByVal context As OpcReadAttributeValueContext, _
        ByVal value As OpcAttributeValue(Of String)) As OpcAttributeValue(Of String)
    Return If(ReadDescriptionFromDataSource(context.Node), value)
End Function


'DOC
machineIsRunningNode.ReadVariableValueCallback = AddressOf HandleReadVariableValue
...
Private Function HandleReadVariableValue( _
        ByVal context As OpcReadVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    Return If(ReadValueFromDataSource(context.Node), value)
End Function

