'DOC
Dim result As OpcStatus = client.WriteNode("ns=2;s=Machine/Job/Cancel", True)

'DOC
Dim commands As OpcWriteNode() = New OpcWriteNode() { _
    New OpcWriteNode("ns=2;s=Machine/Job/Number", "0002"), _
    New OpcWriteNode("ns=2;s=Machine/Job/Name", "MAN_F01_78910"), _
    New OpcWriteNode("ns=2;s=Machine/Job/Speed", 1220.5) _
}

Dim results As OpcStatusCollection = client.WriteNodes(commands)

'DOC
client.WriteNode("ns=2;s=Machine/IsRunning", OpcAttribute.DisplayName, "IsActive")

'DOC
Dim commands As OpcWriteNode() = New OpcWriteNode() { _
    New OpcWriteNode("ns=2;s=Machine/Job/Number", OpcAttribute.DisplayName, "Serial"), _
    New OpcWriteNode("ns=2;s=Machine/Job/Name", OpcAttribute.DisplayName, "Description"), _
    New OpcWriteNode("ns=2;s=Machine/Job/Speed", OpcAttribute.DisplayName, "Rotations per Second") _
}

Dim results As OpcStatusCollection = client.WriteNodes(commands)

'DOC
'Write the elements with the index 2 to 4 of the array.
Dim result As OpcStatus = client.WriteNode( _
        "ns=2;s=Machine/Job/Speeds", _
        New OpcRange(2, 4), _
        New Integer() {1200, 1300, 1400})

'DOC
'Always send the current time as the source timestamp of the values to write.
client.Services.WriteNodes.TimestampSource = OpcTimestampSource.UtcNow
