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

'DOC
'Create a browse command to browse all hierarchical references.
Dim browse = New OpcBrowseNode( _
        nodeId:=OpcNodeId.Parse("ns=2;s=Machine"), _
        degree:=OpcBrowseNodeDegree.Generation)

'Create a browse command to browse specific types of references.
Dim browse = New OpcBrowseNode( _
        nodeId:=OpcNodeId.Parse("ns=2;s=Machine"), _
        degree:=OpcBrowseNodeDegree.Generation, _
        referenceTypes:={ _
            OpcReferenceType.Organizes, _
            OpcReferenceType.HasComponent, _
            OpcReferenceType.HasProperty _
        })

'Reduce browsing to the smallest possible amount of data.
browse.Options = OpcBrowseOptions.IncludeReferenceTypeId _
        Or OpcBrowseOptions.IncludeBrowseName

Dim node = client.BrowseNode(browse)

For Each childNode In node.Children()
    'Continue recursively...
Next
