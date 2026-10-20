'DOC
Private Shared ReadOnly data As BlockingCollection(Of String) = New BlockingCollection(Of String)()

'DOC
Dim processDataCancellation = New CancellationTokenSource()
Dim processDataThread = New Thread(Sub() ProcessData(processDataCancellation.Token))

processDataThread.Start()

'DOC
Using client = New OpcClient("<address>")
    client.Connect()
    client.SubscribeDataChange("<nodeId>", AddressOf HandleDataChanged)

    ...
End Using

'DOC
processDataCancellation.Cancel()
processDataThread.Join()

'DOC
Private Shared Sub HandleDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    data.Add(e.Item.Value.As(Of String)())
End Sub

'DOC
Private Shared Sub ProcessData(ByVal token As CancellationToken)
    While Not token.IsCancellationRequested
        Try
            ' Insert your data processing here.
            Debug.WriteLine(data.Take(token))
        Catch ex As OperationCanceledException
            ' Can be ignored, because of the while loop will exit the method.
        End Try
    End While
End Sub
