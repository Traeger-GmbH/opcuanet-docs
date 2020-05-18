'DOC
Dim machineNode As New OpcFolderNode("Machine")
Dim machineIsRunningNode = New OpcDataVariableNode(Of Boolean)(machineNode, "IsRunning")
 
'Note: An enumerable of nodes can be also passed.
Dim server = New OpcServer("opc.tcp://localhost:4840/", machineNode)

'DOC
Public Class MyNodeManager
    Inherits OpcNodeManager

    Public Sub New()
        MyBase.New("http://mynamespace/")
    End Sub
End Class

'DOC
Protected Overrides Iterator Function CreateNodes(ByVal references As OpcNodeReferenceCollection) As IEnumerable(Of IOpcNode)
    'Define custom root node.
    Dim machineNode = New OpcFolderNode(New OpcName("Machine", Me.DefaultNamespaceIndex))

    'Add custom root node to the Objects-Folder (the root of all server nodes):
    references.Add(machineNode, OpcObjectTypes.ObjectsFolder)

    'Add custom sub node beneath of the custom root node:
    Dim isMachineRunningNode = New OpcDataVariableNode(Of Boolean)(machineNode, "IsRunning")

    'Return each custom root node using yield return.
    Yield machineNode
End Function


'DOC
'Note: An enumerable of node managers can be also passed.
Dim server = New OpcServer("opc.tcp://localhost:4840/", New MyNodeManager())
