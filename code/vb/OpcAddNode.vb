'DOC
Dim result As OpcAddNodeResult = client.AddNode(New OpcAddFolderNode( _
        name:="Jobs", _
        nodeId:=OpcNodeId.Null, _
        parentNodeId:="ns=2;s=Machine"))

'DOC
If result.IsGood
    Console.WriteLine("NodeId of 'Jobs': {0}", result.NodeId)
Else
    Console.WriteLine($"Failed to add node: {0}", result.Description)
End If

'DOC
Dim jobsNodeId As OpcNodeId = result.NodeId

Dim results As OpcAddNodeResultCollection = client.AddNodes( _
        New OpcAddDataVariableNode(Of String)("CurrentJob", jobsNodeId), _
        New OpcAddDataVariableNode(Of String)("NextJob", jobsNodeId), _
        New OpcAddDataVariableNode(Of Integer)("NumberOfJobs", jobsNodeId))

'DOC
Dim results As OpcAddNodeResultCollection = client.AddNodes( _
        New OpcAddObjectNode( _
                "JOB001", _
                nodeId:=OpcNodeId.Null, _
                parentNodeId:=jobsNodeId, _
                New OpcAddDataVariableNode(Of SByte)("Status", -1), _
                New OpcAddDataVariableNode(Of String)("Serial", "J01-DX-11.001"), _
                New OpcAddAnalogItemNode(Of Single)("Speed", 1200F) With {
                    .EngineeringUnit = New OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
                    .EngineeringUnitRange = New OpcValueRange(5400, 0),
                    .Definition = "DB100.DBW 0"
                }, _
                New OpcAddObjectNode( _
                        "Setup", _
                        New OpcAddPropertyNode(Of Boolean)("UseCutter"), _
                        New OpcAddPropertyNode(Of Boolean)("UseDrill")), _
                New OpcAddObjectNode( _
                        "Schedule", _
                        New OpcAddPropertyNode(Of DateTime)("EarliestStartTime"), _
                        New OpcAddPropertyNode(Of DateTime)("LatestStartTime"), _
                        New OpcAddPropertyNode(Of TimeSpan)("EstimatedRunTime"))), _
        New OpcAddObjectNode( _
                "JOB002", _
                nodeId:=OpcNodeId.Null, _
                parentNodeId:=jobsNodeId, _
                New OpcAddDataVariableNode(Of SByte)("Status", -1), _
                New OpcAddDataVariableNode(Of String)("Serial", "J01-DX-53.002"), _
                New OpcAddAnalogItemNode(Of Single)("Speed", 3210F) With {
                    .EngineeringUnit = New OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
                    .EngineeringUnitRange = New OpcValueRange(5400, 0),
                    .Definition = "DB200.DBW 0" _
                }, _
                New OpcAddObjectNode( _
                        "Setup", _
                        New OpcAddPropertyNode(Of Boolean)("UseCutter"), _
                        New OpcAddPropertyNode(Of Boolean)("UseDrill")), _
                New OpcAddObjectNode( _
                        "Schedule", _
                        New OpcAddPropertyNode(Of DateTime)("EarliestStartTime"), _
                        New OpcAddPropertyNode(Of DateTime)("LatestStartTime"), _
                        New OpcAddPropertyNode(Of TimeSpan)("EstimatedRunTime"))))

'DOC
Dim jobsNodeId = result.NodeId
 
Dim job = New OpcAddObjectNode( _
        name:="JOB003", _
        nodeId:=OpcNodeId.Null, _
        parentNodeId:=jobsNodeId)
 
job.Children.Add(New OpcAddDataVariableNode(Of SByte)("Status", -1))
job.Children.Add(New OpcAddDataVariableNode(Of String)("Serial", "J01-DX-78.003"))
job.Children.Add(new OpcAddAnalogItemNode(Of Single)("Speed", 1200f) With {
    .EngineeringUnit = new OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
    .EngineeringUnitRange = new OpcValueRange(5400, 0),
    .Definition = "DB100.DBW 0" _
})
 
Dim setup = New OpcAddObjectNode("Setup")
setup.Children.Add(New OpcAddPropertyNode(Of Boolean)("UseCutter"))
setup.Children.Add(New OpcAddPropertyNode(Of Boolean)("UseDrill"))

job.Children.Add(setup)

Dim schedule = New OpcAddObjectNode("Schedule")
schedule.Children.Add(New OpcAddPropertyNode(Of DateTime)("EarliestStartTime"))
schedule.Children.Add(New OpcAddPropertyNode(Of DateTime)("LatestStartTime"))
schedule.Children.Add(New OpcAddPropertyNode(Of TimeSpan)("EstimatedRunTime"))

job.Children.Add(schedule)

Dim result As OpcAddNodeResult = client.AddNode(job)

'DOC
client.AddObjectNode(OpcObjectType.DeviceFailureEventType, "FailureInfo")
client.AddVariableNode(OpcVariableType.XYArrayItem, "Coordinates")

'DOC
'Declare Job Type
Dim jobType = OpcAddObjectNode.OfType(OpcNodeId.[Of]("ns=2;s=Types/JobType"))

client.AddNodes( _
        jobType.Create("JOB001", nodeId:=OpcNodeId.Null, parentNodeId:=jobsNodeId), _
        jobType.Create("JOB002", nodeId:=OpcNodeId.Null, parentNodeId:=jobsNodeId), _
        jobType.Create("JOB003", nodeId:=OpcNodeId.Null, parentNodeId:=jobsNodeId), _
        jobType.Create("JOB004", nodeId:=OpcNodeId.Null, parentNodeId:=jobsNodeId))

Dim scheduleNodeId = OpcNodeId.Parse("ns=2;s=Machine/JOB002/Schedule")

'Declare Shift Time Type
Dim shiftTimeType = OpcAddVariableNode.OfType(OpcNodeId.[Of]("ns=2;s=Types/ShiftTimeType"))

Dim result As OpcAddNodeResult = client.AddNode(New OpcAddObjectNode( _
        "ShiftPlanning", _
        nodeId:=OpcNodeId.Null, _
        parentNodeId:=scheduleNodeId, _
        shiftTimeType.Create("Early"), _
        shiftTimeType.Create("Noon"), _
        shiftTimeType.Create("Late")))
