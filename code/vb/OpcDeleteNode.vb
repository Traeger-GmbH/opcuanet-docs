'DOC
Dim result As OpcStatus = client.DeleteNode("ns=2;s=Machine/Jobs")

'DOC
Dim result As OpcStatus = client.DeleteNode( _
        "ns=2;s=Machine/Jobs", _
        includeTargetReferences:=False)

'DOC
Dim results As OpcStatusCollection = client.DeleteNodes( _
        New OpcDeleteNode("ns=2;s=Machine/Jobs/JOB001"), _
        New OpcDeleteNode("ns=2;s=Machine/Jobs/JOB002"), _
        New OpcDeleteNode("ns=2;s=Machine/Jobs/JOB003"))
