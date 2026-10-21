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

'DOC
Protected Overrides Iterator Function ImportNodes() As IEnumerable(Of OpcNodeSet)
    Yield OpcNodeSet.Load(".\umati.xml")
    Yield OpcNodeSet.Load(".\umati-instances.xml")
End Function


'DOC
Dim lampTypeId = OpcNodeId.Parse("ns=2;i=1041")
Dim random = New Random()

Dim umatiManager = OpcNodeSetManager.Create( _
        OpcNodeSet.Load(".\umati.xml"), _
        OpcNodeSet.Load(".\umati-instances.xml"))

umatiManager.ImplementNodeCallback = Sub(context, node)
    Dim variableNode = TryCast(node, OpcVariableNode)

    If variableNode Is Nothing OrElse variableNode.Name <> "2:Status" Then
        Return
    End If

    Dim objectNode = TryCast(variableNode.Parent, OpcObjectNode)

    If objectNode IsNot Nothing AndAlso objectNode.TypeDefinitionId = lampTypeId Then
        variableNode.ReadVariableValueCallback = Function(readContext, value) _
                New OpcVariableValue(Of Object)(random.Next(minValue:=0, maxValue:=2))
    End If
End Sub

'DOC
Protected Overrides Sub ImplementNode(ByVal node As IOpcNode)
    'Keeps the default implementation, e.g. the references to the interfaces of the node.
    MyBase.ImplementNode(node)

    'Implement your Node(s) here.
End Sub


'DOC
umatiManager.ImportOptions = OpcNodeSetImportOptions.CompleteDataTypeDefinitionFields _
        Or OpcNodeSetImportOptions.CreateNonGenericNodeInstances

'DOC
Using stream = File.OpenRead(".\umati.xml")
    Dim nodeSet = OpcNodeSet.Load(stream)

    For Each node In nodeSet.GetNodes()
        Console.WriteLine("{0}: {1}", node.Id, node.DisplayName)
    Next
End Using
