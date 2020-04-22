'DOC
client.DeleteReference( _
        nodeId:="ns=2;s=Machines/MAC03", _
        targetNodeId:="ns=2;s=Plant")

'DOC
client.DeleteReference( _
        nodeId:="ns=2;s=Machines/MAC03", _
        targetNodeId:="ns=2;s=Plant", _
        direction:=OpcReferenceDirection.ChildToParent)

'DOC
client.DeleteReference( _
        nodeId:="ns=2;s=Machines/MAC03", _
        targetNodeId:="ns=2;s=Plant", _
        direction:=OpcReferenceDirection.ChildToParent, _
        referenceType:=OpcReferenceType.HierarchicalReferences)

'DOC
client.DeleteReferences( _
        New OpcDeleteReference("ns=2;s=Machines/MAC01", "ns=2;s=Plant01"), _
        New OpcDeleteReference("ns=2;s=Machines/MAC02", "ns=2;s=Plant01"), _
        New OpcDeleteReference("ns=2;s=Machines/MAC03", "ns=2;s=Plant02"))
