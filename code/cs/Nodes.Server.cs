// DOC
// Define the node identifier associated with the custom data type.
[OpcDataType(id: "MachineStatus", namespaceIndex: 2)]
internal enum MachineStatus : int
{
    Unknown = 0,
    Stopped = 1,
    Started = 2,
    Waiting = 3,
    Suspended = 4
}

...

// MyNodeManager.cs
protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
{
    ...

    // Publish a new data type node using the custom type.
    return new IOpcNode[] { ..., new OpcDataTypeNode<MachineStatus>() };
}

// DOC
[OpcDataType(id: "MachineStatus", namespaceIndex: 2)]
internal enum MachineStatus : int
{
    Unknown = 0,
    Stopped = 1,
    Started = 2,
    
    [OpcEnumMember("Paused by Job")]
    WaitingForOrders = 3,

    [OpcEnumMember("Paused by Operator")]
    Suspended = 4,
}

// DOC
// Node of the type Int32
var variable1Node = new OpcDataVariableNode<int>(machineNode, "Var1");

// Node of the type Int16
var variable2Node = new OpcDataVariableNode<short>(machineNode, "Var2");

// Node of the type String
var variable3Node = new OpcDataVariableNode<string>(machineNode, "Var3");

// Node of the type float-array
var variable4Node = new OpcDataVariableNode<float[]>(machineNode, "Var4", new float[] { 0.1f, 0.5f });

// Node of the type MachineStatus enum
var variable5Node = new OpcDataVariableNode<MachineStatus>(machineNode, "Var5");

// DOC
var statusNode = new OpcDataItemNode<int>(machineNode, "Status");
statusNode.Definition = "Status Code in low word, Progress Code in high word encoded in BCD";

// DOC
var temperatureNode = new OpcAnalogItemNode<float>(machineNode, "Temperature");

temperatureNode.InstrumentRange = new OpcValueRange(80.0, -40.0);
temperatureNode.EngineeringUnit = new OpcEngineeringUnitInfo(4408652, "°C", "degree Celsius");
temperatureNode.EngineeringUnitRange = new OpcValueRange(70.8, 5.0);

// DOC
// Define the node identifier associated with the custom data type.
[OpcDataType(id: "MachineStatus", namespaceIndex: 2)]
internal enum MachineStatus : int
{
    Unknown = 0,
    Stopped = 1,
    Started = 2,
    Waiting = 3,
    Suspended = 4
}
 
...
 
// MyNodeManager.cs
protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
{
    ...
 
    // Publish a new data type node using the custom type.
    return new IOpcNode[] { ..., new OpcDataTypeNode<MachineStatus>() };
}

// DOC
[OpcDataType(id: "MachineStatus", namespaceIndex: 2)]
[OpcEnum(OpcEnumType.Values)]
internal enum MachineStatus : int
{
    Unknown = 0,
    Stopped = 1,
    Started = 2,
 
    [OpcEnumMember("WaitingForOrders", Description = "Paused by Job")]
    WaitingForOrders = 3,
 
    [OpcEnumMember("Suspended", Description = "Paused by Operator")]
    Suspended = 4
}

// DOC
var doorNode = new OpcTwoStateDiscreteNode(machineNode, "DoorOpen", value: false);
doorNode.TrueState = "Open";
doorNode.FalseState = "Closed";
 
var modeNode = new OpcMultiStateDiscreteNode<uint>(machineNode, "Mode", value: 0u);
modeNode.EnumStrings = new OpcText[] { "Manual", "Automatic", "Service" };
 
var errorNode = new OpcMultiStateValueDiscreteNode<int>(machineNode, "Error", value: 0);
errorNode.EnumValues = new OpcEnumValue[] {
    new OpcEnumValue("None", value: 0),
    new OpcEnumValue("Overheated", value: 10)
};

// DOC
var speedNode = new OpcAnalogUnitNode<double>(machineNode, "Speed", value: 0.0);
 
speedNode.EngineeringUnit = new OpcEngineeringUnitInfo(
        unitId: 5067859,
        displayName: "m/s",
        description: "metre per second");
