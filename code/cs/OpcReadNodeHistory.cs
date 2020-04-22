// DOC
var startTime = new DateTime(2017, 2, 16, 10, 0, 0);
var history = client.ReadNodeHistory(
        startTime, null, "ns=2;s=Machine/Job/Speed");

// DOC
var endTime = new DateTime(2017, 2, 16, 15, 0, 0);
var history = client.ReadNodeHistory(
        null, endTime, "ns=2;s=Machine/Job/Speed");

// DOC
var startTime = new DateTime(2017, 2, 16, 10, 0, 0);
var endTime = new DateTime(2017, 2, 16, 15, 0, 0);
 
var history = client.ReadNodeHistory(
        startTime, endTime, "ns=2;s=Machine/Job/Speed");

// DOC
foreach (var value in history) {
    Console.WriteLine(
            "{0}: {1}",
            value.Timestamp,
            value);
}

// DOC
var historyNavigator = client.ReadNodeHistory(
        10, "ns=2;s=Machine/Job/Speed");

// DOC
var startTime = new DateTime(2017, 2, 16, 15, 0, 0);
var historyNavigator = client.ReadNodeHistory(
        startTime, 10, "ns=2;s=Machine/Job/Speed");

// DOC
var endTime = new DateTime(2017, 2, 16, 15, 0, 0);
var historyNavigator = client.ReadNodeHistory(
        null, endTime, 10, "ns=2;s=Machine/Job/Speed");

// DOC
var startTime = new DateTime(2017, 2, 16, 10, 0, 0);
var endTime = new DateTime(2017, 2, 16, 15, 0, 0);
 
var historyNavigator = client.ReadNodeHistory(
        startTime, endTime, 10, "ns=2;s=Machine/Job/Speed");

// DOC
do {
    foreach (var value in historyNavigator) {
        Console.WriteLine(
                "{0}: {1}",
                value.Timestamp,
                value);
    }
} while (historyNavigator.MoveNextPage());
 
historyNavigator.Close();

// DOC
using (historyNavigator) {
    do {
        foreach (var value in historyNavigator) {
            Console.WriteLine(
                    "{0}: {1}",
                    value.Timestamp,
                    value);
        }
    } while (historyNavigator.MoveNextPage());
}

// DOC
var minSpeed = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Minimum,
        "ns=2;s=Machine/Job/Speed");

// DOC
var avgSpeed = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Average,
        "ns=2;s=Machine/Job/Speed");

// DOC
var maxSpeed = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Maximum,
        "ns=2;s=Machine/Job/Speed");
