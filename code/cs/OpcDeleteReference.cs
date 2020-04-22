// DOC
client.DeleteReference(
        nodeId: "ns=2;s=Machines/MAC03",
        targetNodeId: "ns=2;s=Plant");

// DOC
client.DeleteReference(
        nodeId: "ns=2;s=Machines/MAC03",
        targetNodeId: "ns=2;s=Plant",
        direction: OpcReferenceDirection.ChildToParent);

// DOC
client.DeleteReference(
        nodeId: "ns=2;s=Machines/MAC03",
        targetNodeId: "ns=2;s=Plant",
        direction: OpcReferenceDirection.ChildToParent, 
        referenceType: OpcReferenceType.HierarchicalReferences);

// DOC
client.DeleteReferences(
        new OpcDeleteReference("ns=2;s=Machines/MAC01", "ns=2;s=Plant01"),
        new OpcDeleteReference("ns=2;s=Machines/MAC02", "ns=2;s=Plant01"),
        new OpcDeleteReference("ns=2;s=Machines/MAC03", "ns=2;s=Plant02"));
