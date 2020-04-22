// DOC
OpcAddNodeResult result = client.AddNode(new OpcAddFolderNode(
    name: "Jobs",
    nodeId: OpcNodeId.Null,
    parentNodeId: "ns=2;s=Machine"));

// DOC
if (result.IsGood)
    Console.WriteLine($"NodeId of 'Jobs': {result.NodeId}");
else
    Console.WriteLine($"Failed to add node: {result.Description}");


// DOC
OpcNodeId jobsNodeId = result.NodeId;
 
OpcAddNodeResultCollection results = client.AddNodes(
        new OpcAddDataVariableNode<string>("CurrentJob", jobsNodeId),
        new OpcAddDataVariableNode<string>("NextJob", jobsNodeId),
        new OpcAddDataVariableNode<int>("NumberOfJobs", jobsNodeId));

// DOC
OpcAddNodeResultCollection results = client.AddNodes(
        new OpcAddObjectNode(
                "JOB001",
                nodeId: OpcNodeId.Null,
                parentNodeId: jobsNodeId,
                new OpcAddDataVariableNode<sbyte>("Status", -1),
                new OpcAddDataVariableNode<string>("Serial", "J01-DX-11.001"),
                new OpcAddAnalogItemNode<float>("Speed", 1200f) {
                    EngineeringUnit = new OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
                    EngineeringUnitRange = new OpcValueRange(5400, 0),
                    Definition = "DB100.DBW 0"
                },
                new OpcAddObjectNode(
                        "Setup",
                        new OpcAddPropertyNode<bool>("UseCutter"),
                        new OpcAddPropertyNode<bool>("UseDrill")),
                new OpcAddObjectNode(
                        "Schedule",
                        new OpcAddPropertyNode<DateTime>("EarliestStartTime"),
                        new OpcAddPropertyNode<DateTime>("LatestStartTime"),
                        new OpcAddPropertyNode<TimeSpan>("EstimatedRunTime"))),
        new OpcAddObjectNode(
                "JOB002",
                nodeId: OpcNodeId.Null,
                parentNodeId: jobsNodeId,
                new OpcAddDataVariableNode<sbyte>("Status", -1),
                new OpcAddDataVariableNode<string>("Serial", "J01-DX-53.002"),
                new OpcAddAnalogItemNode<float>("Speed", 3210f) {
                    EngineeringUnit = new OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
                    EngineeringUnitRange = new OpcValueRange(5400, 0),
                    Definition = "DB200.DBW 0"
                },
                new OpcAddObjectNode(
                        "Setup",
                        new OpcAddPropertyNode<bool>("UseCutter"),
                        new OpcAddPropertyNode<bool>("UseDrill")),
                new OpcAddObjectNode(
                        "Schedule",
                        new OpcAddPropertyNode<DateTime>("EarliestStartTime"),
                        new OpcAddPropertyNode<DateTime>("LatestStartTime"),
                        new OpcAddPropertyNode<TimeSpan>("EstimatedRunTime"))));

// DOC
var jobsNodeId = result.NodeId;
 
var job = new OpcAddObjectNode(
        name: "JOB003",
        nodeId: OpcNodeId.Null,
        parentNodeId: jobsNodeId);
 
job.Children.Add(new OpcAddDataVariableNode<sbyte>("Status", -1));
job.Children.Add(new OpcAddDataVariableNode<string>("Serial", "J01-DX-78.003"));
job.Children.Add(new OpcAddAnalogItemNode<float>("Speed", 1200f) {
    EngineeringUnit = new OpcEngineeringUnitInfo(5067859, "m/s", "metre per second"),
    EngineeringUnitRange = new OpcValueRange(5400, 0),
    Definition = "DB100.DBW 0"
});
 
var setup = new OpcAddObjectNode("Setup");
setup.Children.Add(new OpcAddPropertyNode<bool>("UseCutter"));
setup.Children.Add(new OpcAddPropertyNode<bool>("UseDrill"));
 
job.Children.Add(setup);
 
var schedule = new OpcAddObjectNode("Schedule");
schedule.Children.Add(new OpcAddPropertyNode<DateTime>("EarliestStartTime"));
schedule.Children.Add(new OpcAddPropertyNode<DateTime>("LatestStartTime"));
schedule.Children.Add(new OpcAddPropertyNode<TimeSpan>("EstimatedRunTime"));
 
job.Children.Add(schedule);
 
OpcAddNodeResult result = client.AddNode(job);

// DOC
client.AddObjectNode(OpcObjectType.DeviceFailureEventType, "FailureInfo");
client.AddVariableNode(OpcVariableType.XYArrayItem, "Coordinates");

// DOC
// Declare Job Type
var jobType = OpcAddObjectNode.OfType(OpcNodeId.Of("ns=2;s=Types/JobType"));
 
client.AddNodes(
        jobType.Create("JOB001", nodeId: OpcNodeId.Null, parentNodeId: jobsNodeId),
        jobType.Create("JOB002", nodeId: OpcNodeId.Null, parentNodeId: jobsNodeId),
        jobType.Create("JOB003", nodeId: OpcNodeId.Null, parentNodeId: jobsNodeId),
        jobType.Create("JOB004", nodeId: OpcNodeId.Null, parentNodeId: jobsNodeId));
 
var scheduleNodeId = OpcNodeId.Parse("ns=2;s=Machine/JOB002/Schedule");
 
// Declare Shift Time Type
var shiftTimeType = OpcAddVariableNode.OfType(OpcNodeId.Of("ns=2;s=Types/ShiftTimeType"));
 
OpcAddNodeResult result = client.AddNode(new OpcAddObjectNode(
        "ShiftPlanning",
        nodeId: OpcNodeId.Null,
        parentNodeId: scheduleNodeId,
        shiftTimeType.Create("Early"),
        shiftTimeType.Create("Noon"),
        shiftTimeType.Create("Late")));
