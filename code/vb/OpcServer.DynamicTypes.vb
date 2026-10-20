' DOC
server.UseDynamic = True

' DOC
Dim machineDataTypeId = New OpcNodeId("MachineData", 2)

Dim machineDataType = OpcStruct.DefineType(
        machineDataTypeId,
        New OpcName("MachineData", 2),
        New OpcStructField("Speed", OpcDataTypes.Int32),
        New OpcStructField("JobName", OpcDataTypes.String),
        New OpcStructField("Temperatures", OpcDataTypes.Double).AsArray(),
        New OpcStructField("Comment", OpcDataTypes.String, "Optional comment.").AsOptional())

' DOC
Dim jobKindId = New OpcNodeId("JobKind", 2)

Dim jobKind = OpcEnum.DefineStrings(
        jobKindId,
        New OpcName("JobKind", 2),
        New OpcEnumField("Production"),
        New OpcEnumField("Maintenance"))

Dim errorCode = OpcEnum.DefineValues(
        New OpcNodeId("ErrorCode", 2),
        New OpcName("ErrorCode", 2),
        New OpcEnumField("None", 0),
        New OpcEnumField("Overheated", 10, "The temperature exceeded the limit."),
        New OpcEnumField("OutOfMaterial", 20, "The material supply is empty."))

' DOC
' MyNodeManager.CreateNodes(...)
Dim machineDataNode = New OpcDataVariableNode(machineNode, "Data")
machineDataNode.DataTypeId = machineDataTypeId

Dim jobKindNode = New OpcDataVariableNode(machineNode, "JobKind")
jobKindNode.DataTypeId = jobKindId

' DOC
server.Start()

' The data type nodes are added after the server has been started.
nodeManager.AddNode(OpcDataTypeNode.Create(machineDataType))
nodeManager.AddNode(OpcDataTypeNode.Create(jobKind))
nodeManager.AddNode(OpcDataTypeNode.Create(errorCode))

' DOC
Dim dataType = server.GetDataType(machineDataTypeId)
Dim data = New OpcDataObject(dataType)

data("Speed").Value = 1200
data("JobName").Value = "JOB0815"
data("Temperatures").Value = New Double() {20.5, 21.0, 21.3}

machineDataNode.Value = data
machineDataNode.ApplyChanges(server.SystemContext)

' Enumeration values are numbers (index or value of the field).
jobKindNode.Value = 1 ' Maintenance
jobKindNode.ApplyChanges(server.SystemContext)

' DOC
Dim extendedDataType = OpcStruct.DefineSubType(
        machineDataTypeId,
        New OpcNodeId("ExtendedMachineData", 2),
        New OpcName("ExtendedMachineData", 2),
        New OpcStructField("Operator", OpcDataTypes.String))
