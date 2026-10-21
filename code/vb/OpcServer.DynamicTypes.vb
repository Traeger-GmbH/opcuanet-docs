'DOC
server.UseDynamic = True

'DOC
Dim machineDataTypeId = New OpcNodeId("MachineData", namespaceIndex:=2)

Dim machineDataType = OpcStruct.DefineType( _
        machineDataTypeId, _
        New OpcName("MachineData", namespaceIndex:=2), _
        New OpcStructField("Speed", OpcDataTypes.Int32), _
        New OpcStructField("JobName", OpcDataTypes.String), _
        New OpcStructField("Temperatures", OpcDataTypes.Double).AsArray(), _
        New OpcStructField("Comment", OpcDataTypes.String, description:="Optional comment.") _
                .AsOptional())

'DOC
Dim jobKindId = New OpcNodeId("JobKind", namespaceIndex:=2)

Dim jobKind = OpcEnum.DefineStrings( _
        jobKindId, _
        New OpcName("JobKind", namespaceIndex:=2), _
        New OpcEnumField("Production"), _
        New OpcEnumField("Maintenance"))

Dim errorCode = OpcEnum.DefineValues( _
        New OpcNodeId("ErrorCode", namespaceIndex:=2), _
        New OpcName("ErrorCode", namespaceIndex:=2), _
        New OpcEnumField("None", value:=0), _
        New OpcEnumField("Overheated", value:=10, description:="The temperature exceeded the limit."), _
        New OpcEnumField("OutOfMaterial", value:=20, description:="The material supply is empty."))

'DOC
'MyNodeManager.CreateNodes(...)
Dim machineDataNode = New OpcDataVariableNode(machineNode, "Data")
machineDataNode.DataTypeId = machineDataTypeId

Dim jobKindNode = New OpcDataVariableNode(machineNode, "JobKind")
jobKindNode.DataTypeId = jobKindId

'DOC
server.Start()

'The data type nodes are added after the server has been started.
nodeManager.AddNode(OpcDataTypeNode.Create(machineDataType))
nodeManager.AddNode(OpcDataTypeNode.Create(jobKind))
nodeManager.AddNode(OpcDataTypeNode.Create(errorCode))

'DOC
Dim dataType = server.GetDataType(machineDataTypeId)
Dim data = New OpcDataObject(dataType)

data("Speed").Value = 1200
data("JobName").Value = "JOB0815"
data("Temperatures").Value = New Double() {20.5, 21.0, 21.3}

machineDataNode.Value = data
machineDataNode.ApplyChanges(server.SystemContext)

'Enumeration values are numbers (index or value of the field), here: Maintenance.
jobKindNode.Value = 1
jobKindNode.ApplyChanges(server.SystemContext)

'DOC
Dim extendedDataType = OpcStruct.DefineSubType( _
        machineDataTypeId, _
        New OpcNodeId("ExtendedMachineData", namespaceIndex:=2), _
        New OpcName("ExtendedMachineData", namespaceIndex:=2), _
        New OpcStructField("Operator", OpcDataTypes.String))
