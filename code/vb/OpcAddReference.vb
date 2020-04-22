'DOC
client.AddReference( _
        "ns=2;s=Machines/MAC01", _
        targetNodeId:="ns=2;s=Plant", _
        targetNodeCategory:=OpcNodeCategory.Object)

'DOC
client.AddReference( _
        "ns=2;s=Machines/MAC01", _
        targetNodeId:="ns=2;s=Plant", _
        targetNodeCategory:=OpcNodeCategory.Object, _
        direction:=OpcReferenceDirection.ParentToChild, _
        referenceType:=OpcReferenceType.Organizes)

'DOC
client.AddReferences( _
        New OpcAddReference("ns=2;s=Machines/MAC01", "ns=2;s=Plant01", OpcNodeCategory.Object), _
        New OpcAddReference("ns=2;s=Machines/MAC02", "ns=2;s=Plant01", OpcNodeCategory.Object), _
        New OpcAddReference("ns=2;s=Machines/MAC03", "ns=2;s=Plant02", OpcNodeCategory.Object))
