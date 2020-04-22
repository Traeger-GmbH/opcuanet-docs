'DOC
Dim machineStatusNode = TryCast(client.BrowseNode("ns=2;s=MachineStatus"), OpcTypeNodeInfo)

If machineStatusNode IsNot Nothing AndAlso machineStatusNode.IsEnum Then
    Dim members = machineStatusNode.GetEnumMembers()

    For Each member In members
        Console.WriteLine(member.Name)
    Next
End If

'DOC
Dim machineStatusNode = TryCast(client.BrowseNode("ns=2;s=Machine/Status"), OpcVariableNodeInfo)

If machineStatusNode IsNot Nothing Then
    Console.WriteLine("AccessLevel: {0}", machineStatusNode.AccessLevel)
    Console.WriteLine("UserAccessLevel: {0}", machineStatusNode.UserAccessLevel)
    ...
End If

'DOC
Dim dataTypeNode = machineStatusNode.DataType

If dataTypeNode.IsSystemType Then
    ...
End If

'DOC
Dim temperatureNode = TryCast(client.BrowseNode("ns=2;s=Machine/Temperature"), OpcAnalogItemNodeInfo)

If temperatureNode IsNot Nothing Then
    Console.WriteLine("InstrumentRange: {0}", temperatureNode.InstrumentRange)

    Console.WriteLine("EngineeringUnit: {0}", temperatureNode.EngineeringUnit)
    Console.WriteLine("EngineeringUnitRange: {0}", temperatureNode.EngineeringUnitRange)
End If
