'DOC
Dim logFilePath As String = Path.Combine(AppContext.BaseDirectory, "opcua-trace.log")

Utils.SetTraceLog(logFilePath, deleteExisting:=True)
Utils.SetTraceMask( _
        Utils.TraceMasks.Error _
        Or Utils.TraceMasks.Information _
        Or Utils.TraceMasks.Security _
        Or Utils.TraceMasks.StartStop)

Utils.SetTraceOutput(Utils.TraceOutput.FileOnly)

'DOC
AddHandler Utils.Tracing.TraceEventHandler, AddressOf HandleTraceEvent

Private Shared Sub HandleTraceEvent( _
        ByVal sender As Object, _
        ByVal e As TraceEventArgs)
    If (e.TraceMask And (Utils.TraceMasks.Error Or Utils.TraceMasks.Security)) = 0 Then Return

    Console.WriteLine( _
            "{0} {1}", _
            FormatTraceText(e), _
            e.Exception?.Message)
End Sub

Private Shared Function FormatTraceText(ByVal e As TraceEventArgs) As String
    'Messages without arguments, e.g. those of exceptions, arrive formatted already.
    If e.Arguments Is Nothing OrElse e.Arguments.Length = 0 Then Return e.Format

    Return String.Format(CultureInfo.InvariantCulture, e.Format, e.Arguments)
End Function





'DOC
Dim traceMessages = New BlockingCollection(Of String)()

'The traced code waits for the handler, so the handler only queues the text.
AddHandler Utils.Tracing.TraceEventHandler, Sub(sender, e) traceMessages.Add(FormatTraceText(e))

Dim traceWriter = New Thread(Sub()
    For Each message As String In traceMessages.GetConsumingEnumerable()
        Console.WriteLine(message)
    Next
End Sub)

traceWriter.IsBackground = True
traceWriter.Start()
