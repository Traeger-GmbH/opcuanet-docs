// DOC
foreach (OpcNamespace ns in client.Namespaces)
    Console.WriteLine("{0}: {1}", ns.Index, ns.Value);
 
OpcNodeInfo objects = client.BrowseNode(OpcObjectTypes.ObjectsFolder);
 
foreach (OpcNodeInfo node in objects.Children())
    Console.WriteLine("{0} ({1})", node.DisplayName.Value, node.NodeId);



// DOC
OpcNamespace da = null;
 
foreach (OpcNamespace ns in client.Namespaces) {
    if (ns.Value.EndsWith("/DA"))
        da = ns;
}
 
var tag = new OpcNodeId("0:Channel1.Device1.Tag1", namespaceIndex: da.Index);
 
OpcValue value = client.ReadNode(tag);
OpcStatus result = client.WriteNode(tag, value: 42);
 
OpcSubscription subscription = client.SubscribeDataChange(tag, HandleDataChanged);


// DOC
client.Interfaces.DataAccess.Organization = OpcClassicOrganization.Flat;
client.Interfaces.DataAccess.SupportsBrowseTo = false;
 
client.Connect();

// DOC
DateTime endTime = DateTime.UtcNow;
DateTime startTime = endTime.AddHours(-1);
 
var raw = client.ReadNodeHistory(startTime, endTime, hdaItemId);
var average = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Average,
        hdaItemId);
 
foreach (var historyValue in raw)
    Console.WriteLine("{0}: {1}", historyValue.Timestamp, historyValue.Value);


// DOC
OpcSubscription subscription = client.SubscribeEvent(
        OpcObjectTypes.Server,
        HandleEventReceived);
 
subscription.RefreshConditions();
...
private void HandleEventReceived(object sender, OpcEventReceivedEventArgs e)
{
    var condition = e.Event as OpcAcknowledgeableCondition;
 
    if (condition != null && !condition.IsAcked)
        condition.Acknowledge(client, comment: "Acknowledged by operator");
}

// DOC
var client = new OpcClient("opc.com://remote-host/{E4EBF7FA-CCAC-4125-A611-EAC4981C00EA}");
 
client.Security.UserIdentity = new OpcClientIdentity(userName: "opcuser", password: "secret");
client.Security.AuthenticationLevel = OpcClassicAuthenticationLevel.PacketIntegrity;
client.Security.ImpersonationLevel = OpcClassicImpersonationLevel.Impersonate;
 
client.Connect();

// DOC
var identity = new OpcClassicIdentity(userName: "opcuser", password: "secret");
 
using (var discovery = new OpcClassicDiscoveryClient(host: "remote-host", identity)) {
    foreach (OpcClassicEndpointDescription endpoint in discovery.DiscoverEndpoints()) {
        Console.WriteLine("{0}: {1}", endpoint.Server.Name, endpoint.Server.Uri);
 
        foreach (OpcClassicInterface serverInterface in endpoint.Interfaces)
            Console.WriteLine("  {0}", serverInterface);
    }
}


// DOC
using (var discovery = new OpcClassicDiscoveryClient(host: "remote-host", identity)) {
    OpcClassicApplicationDescriptionCollection servers
            = discovery.DiscoverServers(OpcClassicInterfaces.Da30);
}

// DOC
var client = new OpcClient(endpoint.Server.Uri);
 
foreach (OpcClassicInterface serverInterface in endpoint.Interfaces)
    client.ServerInterfaces.Add(serverInterface);
 
client.Connect();


// DOC
client.Interfaces.UseBreakMonitoring = true;
client.StateChanged += HandleStateChanged;
 
client.Connect();
...
private static void HandleStateChanged(
        object sender,
        OpcClientStateChangedEventArgs e)
{
    Console.WriteLine("{0} -> {1}", e.OldState, e.NewState);
}
