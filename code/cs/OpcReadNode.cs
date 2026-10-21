// DOC
OpcValue isRunning = client.ReadNode("ns=2;s=Machine/IsRunning");

// DOC
OpcReadNode[] commands = new OpcReadNode[] {
    new OpcReadNode("ns=2;s=Machine/Job/Number"),
    new OpcReadNode("ns=2;s=Machine/Job/Name"),
    new OpcReadNode("ns=2;s=Machine/Job/Speed")
};

IEnumerable<OpcValue> job = client.ReadNodes(commands);

// DOC
OpcValue isRunningDisplayName = client.ReadNode("ns=2;s=Machine/IsRunning", OpcAttribute.DisplayName);

// DOC
OpcReadNode[] commands = new OpcReadNode[] {
    new OpcReadNode("ns=2;s=Machine/Job/Number", OpcAttribute.DisplayName),
    new OpcReadNode("ns=2;s=Machine/Job/Name", OpcAttribute.DisplayName),
    new OpcReadNode("ns=2;s=Machine/Job/Speed", OpcAttribute.DisplayName)
};
 
IEnumerable<OpcValue> jobDisplayNames = client.ReadNodes(commands);

// DOC
// Read the elements with the index 2 to 4 of the array.
OpcValue speeds = client.ReadNode(
        "ns=2;s=Machine/Job/Speeds",
        new OpcRange(start: 2, end: 4));
