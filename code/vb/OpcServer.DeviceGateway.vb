'DOC
Public Class PlcNodeManager
    Inherits OpcNodeManager

    Private temperatureNode As OpcDataVariableNode(Of Double)
    Private speedNode As OpcDataVariableNode(Of Integer)

    Public Sub New()
        MyBase.New("http://example.com/plc")
    End Sub

    Protected Overrides Iterator Function CreateNodes( _
            ByVal references As OpcNodeReferenceCollection) As IEnumerable(Of IOpcNode)
        Dim machine = New OpcObjectNode(New OpcName("Machine", Me.DefaultNamespaceIndex))
        references.Add(machine, OpcObjectTypes.ObjectsFolder)

        Me.temperatureNode = New OpcDataVariableNode(Of Double)(machine, "Temperature")
        Me.speedNode = New OpcDataVariableNode(Of Integer)(machine, "Speed")

        'Clients may write the speed; the value is forwarded to the device.
        Me.speedNode.AccessLevel = OpcAccessLevel.CurrentReadOrWrite
        Me.speedNode.UserAccessLevel = OpcAccessLevel.CurrentReadOrWrite
        Me.speedNode.WriteVariableValueCallback = AddressOf Me.HandleWriteSpeed

        Yield machine
    End Function
End Class

'DOC
Public Sub Update(ByVal reading As PlcReading)
    If reading.IsAvailable Then
        Me.temperatureNode.Value = reading.Temperature
        Me.temperatureNode.Status.Update(OpcStatusCode.Good)
        Me.temperatureNode.Timestamp = reading.Timestamp
    Else
        'Keep the last value, but report that it is no longer up to date.
        Me.temperatureNode.Status.Update(OpcStatusCode.BadCommunicationError)
        Me.temperatureNode.Timestamp = DateTime.UtcNow
    End If

    Me.temperatureNode.ApplyChanges(Me.SystemContext)
End Sub



'DOC
Using plcManager = New PlcNodeManager()
    Using server = New OpcServer("opc.tcp://localhost:4840/", plcManager)
        server.Start()

        While Not cancellation.IsCancellationRequested
            plcManager.Update(Plc.ReadTemperature())
            Thread.Sleep(millisecondsTimeout:=1000)
        End While
    End Using
End Using

'DOC
Private Function HandleWriteSpeed( _
        ByVal context As OpcWriteVariableValueContext, _
        ByVal value As OpcVariableValue(Of Object)) As OpcVariableValue(Of Object)
    If Not Plc.TryWriteSpeed(CInt(value.Value)) Then
        'The client receives this status as the result of its write request.
        Return New OpcVariableValue(Of Object)( _
                value.Value, _
                timestamp:=DateTime.UtcNow, _
                statusCode:=OpcStatusCode.BadCommunicationError)
    End If

    Return value
End Function


'DOC
Public Sub Poll()
    'Only query values a client has subscribed to (see MachineNodeManager).
    If Me.IsMonitored(Me.temperatureNode.Id) Then
        Me.Update(Plc.ReadTemperature())
    End If
End Sub
