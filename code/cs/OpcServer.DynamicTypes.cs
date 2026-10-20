// DOC
server.UseDynamic = true;

// DOC
var machineDataTypeId = new OpcNodeId("MachineData", 2);

var machineDataType = OpcStruct.DefineType(
        machineDataTypeId,
        new OpcName("MachineData", 2),
        new OpcStructField("Speed", OpcDataTypes.Int32),
        new OpcStructField("JobName", OpcDataTypes.String),
        new OpcStructField("Temperatures", OpcDataTypes.Double).AsArray(),
        new OpcStructField("Comment", OpcDataTypes.String, "Optional comment.").AsOptional());

// DOC
var jobKindId = new OpcNodeId("JobKind", 2);

var jobKind = OpcEnum.DefineStrings(
        jobKindId,
        new OpcName("JobKind", 2),
        new OpcEnumField("Production"),
        new OpcEnumField("Maintenance"));

var errorCode = OpcEnum.DefineValues(
        new OpcNodeId("ErrorCode", 2),
        new OpcName("ErrorCode", 2),
        new OpcEnumField("None", 0),
        new OpcEnumField("Overheated", 10, "The temperature exceeded the limit."),
        new OpcEnumField("OutOfMaterial", 20, "The material supply is empty."));

// DOC
// MyNodeManager.CreateNodes(...)
var machineDataNode = new OpcDataVariableNode(machineNode, "Data");
machineDataNode.DataTypeId = machineDataTypeId;

var jobKindNode = new OpcDataVariableNode(machineNode, "JobKind");
jobKindNode.DataTypeId = jobKindId;

// DOC
server.Start();

// The data type nodes are added after the server has been started.
nodeManager.AddNode(OpcDataTypeNode.Create(machineDataType));
nodeManager.AddNode(OpcDataTypeNode.Create(jobKind));
nodeManager.AddNode(OpcDataTypeNode.Create(errorCode));

// DOC
var dataType = server.GetDataType(machineDataTypeId);
var data = new OpcDataObject(dataType);

data["Speed"].Value = 1200;
data["JobName"].Value = "JOB0815";
data["Temperatures"].Value = new[] { 20.5, 21.0, 21.3 };

machineDataNode.Value = data;
machineDataNode.ApplyChanges(server.SystemContext);

// Enumeration values are numbers (index or value of the field).
jobKindNode.Value = 1; // Maintenance
jobKindNode.ApplyChanges(server.SystemContext);

// DOC
var extendedDataType = OpcStruct.DefineSubType(
        machineDataTypeId,
        new OpcNodeId("ExtendedMachineData", 2),
        new OpcName("ExtendedMachineData", 2),
        new OpcStructField("Operator", OpcDataTypes.String));
