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

// DOC
// Read the average value of each hour (3600000 milliseconds) within the time window.
var avgSpeedPerHour = client.ReadNodeHistoryProcessed(
        startTime,
        endTime,
        OpcAggregateType.Average,
        3600000,
        "ns=2;s=Machine/Job/Speed");

// DOC
var times = new DateTime[] {
    new DateTime(2017, 2, 16, 10, 0, 0),
    new DateTime(2017, 2, 16, 12, 0, 0)
};

var speeds = client.ReadNodeHistoryAtTime(times, "ns=2;s=Machine/Job/Speed");

foreach (var value in speeds)
    Console.WriteLine("{0}: {1}", value.Timestamp, value);

// DOC
var modifiedSpeeds = client.ReadNodeHistoryModified(
        startTime,
        endTime,
        "ns=2;s=Machine/Job/Speed");

foreach (var value in modifiedSpeeds) {
    Console.WriteLine(
            "{0}: {1} ({2} at {3} by {4})",
            value.Timestamp,
            value,
            value.ModificationType,
            value.ModificationTime,
            value.ModificationUserName);
}
