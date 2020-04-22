// DOC
OpcStatus result = client.DeleteNode("ns=2;s=Machine/Jobs");

// DOC
OpcStatus result = client.DeleteNode(
        "ns=2;s=Machine/Jobs",
        includeTargetReferences: false);

// DOC
OpcStatusCollection results = client.DeleteNodes(
        new OpcDeleteNode("ns=2;s=Machine/Jobs/JOB001"),
        new OpcDeleteNode("ns=2;s=Machine/Jobs/JOB002"),
        new OpcDeleteNode("ns=2;s=Machine/Jobs/JOB003"));
