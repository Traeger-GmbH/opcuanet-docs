Me.variableNode.WriteVariableValueCallback = AddressOf Me.WriteToNode

'Deny write of a specific type of value.
Private Function WriteToNode( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    If Not TypeOf value.Value Is Short Then
        value.Status.Update(OpcStatusCode.BadTypeMismatch)
    End If

    Return value
End Function

'Inline change type of value to the expected type.
Private Function WriteToNode( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    If Not TypeOf value.Value Is Short Then
        value = New OpcVariableValue(Of Object)( _
                Convert.ChangeType(value.Value, GetType(Short)), _
                If(value.Timestamp, DateTime.UtcNow), _
                value.Status)
    End If

    Return value
End Function


Me.variableNode.WriteVariableValueCallback = AddressOf Me.WriteToNode

'Deny write of a specific type of value.
Private Function WriteToNode( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    If Not TypeOf value.Value Is Short Then
        value.Status.Update(OpcStatusCode.BadTypeMismatch)
    End If

    Return value
End Function

'Inline change type of value to the expected type.
Private Function WriteToNode( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    If Not TypeOf value.Value Is Short Then
        value = New OpcVariableValue(Of Object)( _
                Convert.ChangeType(value.Value, GetType(Short)), _
                If(value.Timestamp, DateTime.UtcNow), _
                value.Status)
    End If

    Return value
End Function

