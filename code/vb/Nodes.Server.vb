' DOC
' Define the node identifier associated with the custom data type.
<OpcDataType(id:="MachineStatus", namespaceIndex:=2)>
Friend Enum MachineStatus As Integer
    Unknown = 0
    Stopped = 1
    Started = 2
    Waiting = 3
    Suspended = 4
End Enum

...

' MyNodeManager.vb
Protected Overrides Function CreateNodes(references As OpcNodeReferenceCollection) As IEnumerable(Of IOpcNode)
    
	...
    
    ' Publish a new data type node using the custom type.
    Return New IOpcNode() { ..., New OpcDataTypeNode(Of MachineStatus)() }
End Function


' DOC
<OpcDataType(id:="MachineStatus", namespaceIndex:=2)>
Friend Enum MachineStatus As Integer
    Unknown = 0
    Stopped = 1
    Started = 2
    
    <OpcEnumMember("Paused by Job")>
    WaitingForOrders = 3
    
    <OpcEnumMember("Paused by Operator")>
    Suspended = 4
End Enum


' DOC
' Node of the type Int32
Dim variable1Node As New OpcDataVariableNode(Of Integer)(machineNode, "Var1")

' Node of the type Int16
Dim variable2Node As New OpcDataVariableNode(Of Short)(machineNode, "Var2")

' Node of the type String
Dim variable3Node As New OpcDataVariableNode(Of String)(machineNode, "Var3")

' Node of the type float-array
Dim variable4Node As New OpcDataVariableNode(Of Single())(machineNode, "Var4", New Single() {0.1F, 0.5F})

' Node of the type MachineStatus enum
Dim variable5Node As New OpcDataVariableNode(Of MachineStatus)(machineNode, "Var5")

' DOC
Dim statusNode As New OpcDataItemNode(Of Integer)(machineNode, "Status")
statusNode.Definition = "Status Code in low word, Progress Code in high word encoded in BCD"

' DOC
Dim temperatureNode As New OpcAnalogItemNode(Of Single)(machineNode, "Temperature")

temperatureNode.InstrumentRange = New OpcValueRange(80.0, -40.0)
temperatureNode.EngineeringUnit = New OpcEngineeringUnitInfo(4408652, "°C", "degree Celsius")
temperatureNode.EngineeringUnitRange = New OpcValueRange(70.8, 5.0)
