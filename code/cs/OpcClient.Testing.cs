// DOC
var speedNode = new OpcDataVariableNode<int>("Speed", value: 100);
 
using (var server = new OpcServer("opc.tcp://localhost:48400/", speedNode))
using (var client = new OpcClient("opc.tcp://localhost:48400/")) {
    server.Start();
    client.Connect();
 
    OpcValue value = client.ReadNode(speedNode.Id);
 
    if ((int)value.Value != 100)
        throw new Exception("Unexpected speed.");
}



// DOC
OpcStatus result = client.WriteNode(speedNode.Id, value: 120);
 
if (!result.IsGood || speedNode.Value != 120)
    throw new Exception("The value has not been written.");


// DOC
using (var received = new ManualResetEventSlim()) {
    client.SubscribeDataChange(speedNode.Id, (sender, e) => {
        if (e.Item.Value.As<int>() == 150)
            received.Set();
    });
 
    speedNode.Value = 150;
    speedNode.ApplyChanges(server.SystemContext);
 
    if (!received.Wait(TimeSpan.FromSeconds(5)))
        throw new Exception("No data change received.");
}



// DOC
var machine = new OpcObjectNode("Machine");
var startJobNode = new OpcMethodNode(
        machine,
        name: "StartJob",
        new Func<string, int>(StartJob));
 
using (var server = new OpcServer("opc.tcp://localhost:48401/", machine))
using (var client = new OpcClient("opc.tcp://localhost:48401/")) {
    server.Start();
    client.Connect();
 
    object[] outputs = client.CallMethod(machine.Id, startJobNode.Id, "Job 1");
 
    if ((int)outputs[0] != 42)
        throw new Exception("Unexpected job number.");
}
...
private static int StartJob(string name)
{
    return 42;
}

