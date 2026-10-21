// DOC
OpcStatusCollection results = client.CreateNodeHistory(
        "ns=2;s=Machine/Job/Speed",
        new OpcValue(1200, sourceTimestamp: new DateTime(2017, 2, 16, 10, 0, 0)),
        new OpcValue(1300, sourceTimestamp: new DateTime(2017, 2, 16, 10, 5, 0)));
 
foreach (var result in results) {
    if (result.IsBad)
        Console.WriteLine("Failed to create value: {0}", result.Description);
}


// DOC
OpcStatus result = client.ReplaceNodeHistory(
        "ns=2;s=Machine/Job/Speed",
        new OpcValue(1250, sourceTimestamp: new DateTime(2017, 2, 16, 10, 0, 0)));

// DOC
OpcStatus result = client.UpdateNodeHistory(
        "ns=2;s=Machine/Job/Speed",
        new OpcValue(1250, sourceTimestamp: new DateTime(2017, 2, 16, 10, 0, 0)));

// DOC
// Delete all values within the time window.
OpcStatusCollection results = client.DeleteNodeHistory(
        "ns=2;s=Machine/Job/Speed",
        startTime,
        endTime);
 
// Delete the value of the timestamp specified.
OpcStatus result = client.DeleteNodeHistoryAtTime(
        "ns=2;s=Machine/Job/Speed",
        time: new DateTime(2017, 2, 16, 10, 5, 0));
