// DOC
using (var server = new OpcMachineryServer("opc.tcp://localhost:4840/")) {
    server.Start();
 
    OpcMachineryNodeManager machinery = server.NodeManagers
            .OfType<OpcMachineryNodeManager>()
            .First();
 
    var machine = new OpcObjectNode(machinery.Machines, "CNC-042");
    var identification = new OpcMachineIdentificationNode(machine, "Identification");
 
    identification.Manufacturer.Value = "Example Machines Ltd.";
    identification.SerialNumber.Value = "SN-0042";
    identification.ProductInstanceUri.Value = "urn:example:cnc-0042";
 
    machinery.AddNode(server.SystemContext, machine);
}

// DOC
public class HandmadeMachineManager : OpcNodeManager
{
    private static readonly OpcNodeId HasInterface = OpcNodeId.Parse("i=17603");
 
    public HandmadeMachineManager()
        : base("http://example.com/machines/")
    {
    }
 
    public static OpcServer CreateServer()
    {
        var nodeSets = OpcNodeSetManager.Create(
                OpcNodeSet.Load(@"NodeSets\Opc.Ua.Di.NodeSet2.xml"),
                OpcNodeSet.Load(@"NodeSets\Opc.Ua.IA.NodeSet2.xml"),
                OpcNodeSet.Load(@"NodeSets\Opc.Ua.Machinery.NodeSet2.xml"));
 
        return new OpcServer("opc.tcp://localhost:4840/", nodeSets, new HandmadeMachineManager());
    }
 
    protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
    {
        var di = OpcNamespace.Create("http://opcfoundation.org/UA/DI/");
        var machinery = OpcNamespace.Create("http://opcfoundation.org/UA/Machinery/");
        di.Resolve(this);
        machinery.Resolve(this);
 
        // Machinery: Machines folder (i=1001).
        var machine = new OpcObjectNode("CNC-042");
 
        references.Add(
                machine,
                machinery.GetId(1001),
                direction: OpcReferenceDirection.ChildToParent,
                referenceType: OpcReferenceType.Organizes);
 
        // Machinery: MachineIdentificationType (i=1012).
        var identification = new OpcObjectNode(machine, machinery.GetName("Identification"));
        identification.TypeDefinitionId = machinery.GetId(1012);
 
        // Mandatory properties: names, namespaces and data types from the specification.
        var manufacturer = new OpcTextPropertyNode(
                identification,
                di.GetName("Manufacturer"),
                value: new OpcText("Example Machines Ltd."));
 
        var serialNumber = new OpcPropertyNode<string>(
                identification,
                di.GetName("SerialNumber"),
                id: OpcNodeId.Null,
                value: "SN-0042");
 
        var instanceUri = new OpcPropertyNode<string>(
                identification,
                di.GetName("ProductInstanceUri"),
                id: OpcNodeId.Null,
                value: "urn:example:cnc-0042");
 
        // Interfaces of the type: IMachineVendorNameplateType (i=1010),
        // IMachineTagNameplateType (i=1011).
        references.Add(
                identification,
                machinery.GetId(1010),
                direction: OpcReferenceDirection.ParentToChild,
                referenceTypeId: HasInterface);
 
        references.Add(
                identification,
                machinery.GetId(1011),
                direction: OpcReferenceDirection.ParentToChild,
                referenceTypeId: HasInterface);
 
        yield return machine;
    }
}

// DOC
nodeSets.ImplementNodeCallback = (context, node) => {
    var method = node as OpcMethodNode;
 
    if (method != null && method.Name.Value == "TransferToDevice")
        method.Command = new TransferToDeviceCommand();
};
...
public sealed class TransferToDeviceCommand : IOpcMethodCommand
{
    public bool CanExecute(OpcContext context)
    {
        return true;
    }
 
    public bool CanUserExecute(OpcContext context)
    {
        return true;
    }
 
    public void Execute(
            OpcMethodContext context,
            IList<object> inputArguments,
            IList<object> outputArguments)
    {
        // Order and types of the arguments as defined by the specification.
        outputArguments[0] = 1; // TransferID
        outputArguments[1] = 0; // InitTransferStatus
    }
}



// DOC
public class DeviceTransferServices : OpcTransferServicesNode
{
    public DeviceTransferServices(IOpcNode parent)
        : base(parent, "TransferServices")
    {
    }
 
    public override void TransferToDevice(
            OpcMethodContext methodContext,
            out int transferID,
            out int initTransferStatus)
    {
        transferID = 1;
        initTransferStatus = 0;
    }
}

// DOC
using (var client = new OpcMachineryClient("opc.tcp://localhost:4840/")) {
    client.Connect();
 
    OpcNodeInfo machines = client.BrowseNode(OpcMachineryNodes.MachinesNode);
 
    foreach (OpcNodeInfo machine in machines.Children())
        Console.WriteLine(machine.DisplayName.Value);
}

