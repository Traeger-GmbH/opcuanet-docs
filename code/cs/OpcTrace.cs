// DOC
string logFilePath = Path.Combine(AppContext.BaseDirectory, "opcua-trace.log");

Utils.SetTraceLog(logFilePath, deleteExisting: true);
Utils.SetTraceMask(
        Utils.TraceMasks.Error
        | Utils.TraceMasks.Information
        | Utils.TraceMasks.Security
        | Utils.TraceMasks.StartStop);

Utils.SetTraceOutput(Utils.TraceOutput.FileOnly);

// DOC
Utils.Tracing.TraceEventHandler += HandleTraceEvent;

private static void HandleTraceEvent(
        object sender,
        TraceEventArgs e)
{
    if ((e.TraceMask & (Utils.TraceMasks.Error | Utils.TraceMasks.Security)) == 0)
        return;

    Console.WriteLine(FormatTraceText(e));
}

private static string FormatTraceText(TraceEventArgs e)
{
    // Messages without arguments, e.g. those of exceptions, arrive formatted already.
    if (e.Arguments == null || e.Arguments.Length == 0)
        return e.Format;

    return string.Format(CultureInfo.InvariantCulture, e.Format, e.Arguments);
}

// DOC
var traceMessages = new BlockingCollection<string>();

// The traced code waits for the handler, so the handler only queues the text.
Utils.Tracing.TraceEventHandler += (sender, e) => traceMessages.Add(FormatTraceText(e));

var traceWriter = new Thread(() => {
    foreach (string message in traceMessages.GetConsumingEnumerable()) {
        Console.WriteLine(message);
    }
});

traceWriter.IsBackground = true;
traceWriter.Start();
