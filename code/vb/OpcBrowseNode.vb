'DOC
Dim node = client.BrowseNode(OpcObjectTypes.ObjectsFolder)
Browse(node)
...
Private Sub Browse(ByVal node As OpcNodeInfo, ByVal Optional level As Integer = 0)
    Console.WriteLine("{0}{1}({2})", _
            New String("."c, level * 4), _
            node.Attribute(OpcAttribute.DisplayName).Value, _
            node.NodeId)

    level += 1

    For Each childNode In node.Children()
        Browse(childNode, level)
    Next
End Sub

'DOC
Dim machineNode As OpcNodeInfo = client.BrowseNode("ns=2;s=Machine")

'DOC
Dim jobNode As OpcNodeInfo = machineNode.Child("Job")

For Each childNode In machineNode.Children()
    'Your code to operate on each child node.
Next

'DOC
Dim displayName As OpcAttributeInfo = machineNode.Attribute(OpcAttribute.DisplayName)

For Each attribute In machineNode.Attributes()
    'Your code to operate on each attribute.
Next

'DOC
If childNode.Category = OpcNodeCategory.Method Then
    Dim methodNode = CType(childNode, OpcMethodNodeInfo)

    For Each argument In methodNode.GetInputArguments()
        'Your code to operate on each argument.
    Next
End If
