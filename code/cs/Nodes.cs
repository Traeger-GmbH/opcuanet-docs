// DOC
var machineStatusNode = client.BrowseNode("ns=2;s=MachineStatus") as OpcTypeNodeInfo;
 
if (machineStatusNode != null && machineStatusNode.IsEnum) {
    var members = machineStatusNode.GetEnumMembers();
 
    foreach (var member in members)
        Console.WriteLine(member.Name);
}


// DOC
var machineStatusNode = client.BrowseNode("ns=2;s=Machine/Status") as OpcVariableNodeInfo;
 
if (machineStatusNode != null) {
    Console.WriteLine($"AccessLevel: {machineStatusNode.AccessLevel}");
    Console.WriteLine($"UserAccessLevel: {machineStatusNode.UserAccessLevel}");
    ...
}

// DOC
var dataTypeNode = machineStatusNode.DataType;
 
if (dataTypeNode.IsSystemType) {
    ...
}

// DOC
var temperatureNode = client.BrowseNode("ns=2;s=Machine/Temperature") as OpcAnalogItemNodeInfo;
 
if (temperatureNode != null) {
    Console.WriteLine($"InstrumentRange: {temperatureNode.InstrumentRange}");
 
    Console.WriteLine($"EngineeringUnit: {temperatureNode.EngineeringUnit}");
    Console.WriteLine($"EngineeringUnitRange: {temperatureNode.EngineeringUnitRange}");
}
