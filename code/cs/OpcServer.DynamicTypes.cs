// DOC
server.UseDynamic = true;

// DOC
var machineDataTypeId = new OpcNodeId("MachineData", namespaceIndex: 2);
 
var machineDataType = OpcStruct.DefineType(
        machineDataTypeId,
        new OpcName("MachineData", namespaceIndex: 2),
        new OpcStructField("Speed", OpcDataTypes.Int32),
        new OpcStructField("JobName", OpcDataTypes.String),
        new OpcStructField("Temperatures", OpcDataTypes.Double).AsArray(),
        new OpcStructField("Comment", OpcDataTypes.String, description: "Optional comment.")
                .AsOptional());

// DOC
var jobKindId = new OpcNodeId("JobKind", namespaceIndex: 2);
 
var jobKind = OpcEnum.DefineStrings(
        jobKindId,
        new OpcName("JobKind", namespaceIndex: 2),
        new OpcEnumField("Production"),
        new OpcEnumField("Maintenance"));
 
var errorCode = OpcEnum.DefineValues(
        new OpcNodeId("ErrorCode", namespaceIndex: 2),
        new OpcName("ErrorCode", namespaceIndex: 2),
        new OpcEnumField("None", value: 0),
        new OpcEnumField("Overheated", value: 10, description: "The temperature exceeded the limit."),
        new OpcEnumField("OutOfMaterial", value: 20, description: "The material supply is empty."));

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
data["Temperatures"].Value = new double[] { 20.5, 21.0, 21.3 };
 
machineDataNode.Value = data;
machineDataNode.ApplyChanges(server.SystemContext);
 
// Enumeration values are numbers (index or value of the field), here: Maintenance.
jobKindNode.Value = 1;
jobKindNode.ApplyChanges(server.SystemContext);

// DOC
var extendedDataType = OpcStruct.DefineSubType(
        machineDataTypeId,
        new OpcNodeId("ExtendedMachineData", namespaceIndex: 2),
        new OpcName("ExtendedMachineData", namespaceIndex: 2),
        new OpcStructField("Operator", OpcDataTypes.String));
