// DOC
client.AddReference(
    "ns=2;s=Machines/MAC01",
    targetNodeId: "ns=2;s=Plant",
    targetNodeCategory: OpcNodeCategory.Object);

// DOC
client.AddReference(
        "ns=2;s=Machines/MAC01",
        targetNodeId: "ns=2;s=Plant",
        targetNodeCategory: OpcNodeCategory.Object,
        direction: OpcReferenceDirection.ParentToChild,
        referenceType: OpcReferenceType.Organizes);

// DOC
client.AddReferences(
        new OpcAddReference("ns=2;s=Machines/MAC01", "ns=2;s=Plant01", OpcNodeCategory.Object),
        new OpcAddReference("ns=2;s=Machines/MAC02", "ns=2;s=Plant01", OpcNodeCategory.Object),
        new OpcAddReference("ns=2;s=Machines/MAC03", "ns=2;s=Plant02", OpcNodeCategory.Object));
