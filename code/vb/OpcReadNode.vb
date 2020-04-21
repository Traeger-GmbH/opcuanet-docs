'DOC
Dim isRunning As OpcValue = client.ReadNode("ns=2;s=Machine/IsRunning")

'DOC
Dim commands As OpcReadNode() = New OpcReadNode() { _
        New OpcReadNode("ns=2;s=Machine/Job/Number"), _
        New OpcReadNode("ns=2;s=Machine/Job/Name"), _
        New OpcReadNode("ns=2;s=Machine/Job/Speed") _
}

Dim job As IEnumerable(Of OpcValue) = client.ReadNodes(commands)

'DOC
Dim isRunningDisplayName As OpcValue = client.ReadNode("ns=2;s=Machine/IsRunning", OpcAttribute.DisplayName)

'DOC
Dim commands As OpcReadNode() = New OpcReadNode() { _
        New OpcReadNode("ns=2;s=Machine/Job/Number", OpcAttribute.DisplayName), _
        New OpcReadNode("ns=2;s=Machine/Job/Name", OpcAttribute.DisplayName), _
        New OpcReadNode("ns=2;s=Machine/Job/Speed", OpcAttribute.DisplayName) _
}

Dim jobDisplayNames As IEnumerable(Of OpcValue) = client.ReadNodes(commands)
