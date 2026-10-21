'DOC
Private Shared ReadOnly data As New BlockingCollection(Of String)()

'DOC
Dim processDataThread = New Thread(AddressOf ProcessData)
processDataThread.Start()

'DOC
Using client = New OpcClient("<address>")
    client.Connect()
    client.SubscribeDataChange("<nodeId>", AddressOf HandleDataChanged)

    ...
End Using

'DOC
data.CompleteAdding()
processDataThread.Join()

'DOC
Private Shared Sub HandleDataChanged(ByVal sender As Object, ByVal e As OpcDataChangeReceivedEventArgs)
    data.Add(e.Item.Value.As(Of String)())
End Sub


'DOC
Private Shared Sub ProcessData()
    'Using 'GetConsumingEnumerable' removes an item from the collection, consumes it through
    'the For Each loop and blocks until another item is/has being added to collection or
    ''CompleteAdding' is used to signal the end of the data processing.
    For Each item As String In data.GetConsumingEnumerable()
        'Insert your data processing here.
    Next
End Sub

