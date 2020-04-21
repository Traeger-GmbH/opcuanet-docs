// DOC
OpcStatus result = client.WriteNode("ns=2;s=Machine/Job/Cancel", true);

// DOC
OpcWriteNode[] commands = new OpcWriteNode[] {
    new OpcWriteNode("ns=2;s=Machine/Job/Number", "0002"),
    new OpcWriteNode("ns=2;s=Machine/Job/Name", "MAN_F01_78910"),
    new OpcWriteNode("ns=2;s=Machine/Job/Speed", 1220.5)
};
 
OpcStatusCollection results = client.WriteNodes(commands);

// DOC
client.WriteNode("ns=2;s=Machine/IsRunning", OpcAttribute.DisplayName, "IsActive");

// DOC
OpcWriteNode[] commands = new OpcWriteNode[] {
    new OpcWriteNode("ns=2;s=Machine/Job/Number", OpcAttribute.DisplayName, "Serial"),
    new OpcWriteNode("ns=2;s=Machine/Job/Name", OpcAttribute.DisplayName, "Description"),
    new OpcWriteNode("ns=2;s=Machine/Job/Speed", OpcAttribute.DisplayName, "Rotations per Second")
};
 
OpcStatusCollection results = client.WriteNodes(commands);
