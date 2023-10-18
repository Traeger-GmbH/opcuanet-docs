' DOC
Dim umatiManager = OpcNodeSetManager.Create( _
        OpcNodeSet.Load(".\umati.xml"), _
        OpcNodeSet.Load(".\umati-instances.xml"))

Using server As New OpcServer("opc.tcp://localhost:4840/", umatiManager)
    server.Start()
    ' ...
End Using

' DOC
Protected Overrides Function ImportNodes() As IEnumerable(Of OpcNodeSet)
    Yield OpcNodeSet.Load(".\umati.xml")
    Yield OpcNodeSet.Load(".\umati-instances.xml")
End Function


' DOC
Protected Overrides Sub ImplementNode(node As IOpcNode)
    ' Implement your Node(s) here.
End Sub


' DOC
Private Shared ReadOnly LampTypeId As OpcNodeId = "ns=2;i=1041"
Private random As New Random()

Protected Overrides Sub ImplementNode(node As IOpcNode)
    Dim variableNode = TryCast(node, OpcVariableNode)
    
    If variableNode IsNot Nothing AndAlso variableNode.Name = "2:Status" Then
        Dim objectNode = TryCast(variableNode.Parent, OpcObjectNode)
        If objectNode IsNot Nothing AndAlso objectNode.TypeDefinitionId = LampTypeId Then
            variableNode.ReadVariableValueCallback = Function(context, value) New OpcVariableValue(Of Object)(Me.random.Next(0, 2))
        End If
    End If
End Sub
