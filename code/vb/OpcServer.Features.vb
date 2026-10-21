'DOC
Dim endTime As DateTime = DateTime.UtcNow
Dim startTime As DateTime = endTime.AddHours(-8)

Dim averagePerHour = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Average, _
        3600000, _
        "ns=2;s=Machine/Temperature")

Dim atNoon As OpcHistoryValue = client.ReadNodeHistoryAtTime( _
        DateTime.UtcNow.Date.AddHours(12), _
        "ns=2;s=Machine/Temperature")

'DOC
Public Class MachineNodeManager
    Inherits OpcNodeManager

    Private ReadOnly monitored As New Dictionary(Of OpcNodeId, Integer)()

    Public Sub New()
        MyBase.New("http://example.com/machine")

        AddHandler Me.MonitoredItemCreated, AddressOf Me.HandleMonitoredItemCreated
        AddHandler Me.MonitoredItemDeleted, AddressOf Me.HandleMonitoredItemDeleted
    End Sub

    Public Function IsMonitored(nodeId As OpcNodeId) As Boolean
        SyncLock Me.monitored
            Return Me.monitored.ContainsKey(nodeId)
        End SyncLock
    End Function

    Private Sub HandleMonitoredItemCreated(sender As Object, e As OpcMonitoredItemEventArgs)
        If e.Item.Attribute <> OpcAttribute.Value Then
            Return
        End If

        SyncLock Me.monitored
            Dim count As Integer = 0
            Me.monitored.TryGetValue(e.Item.NodeId, count)
            Me.monitored(e.Item.NodeId) = count + 1
        End SyncLock
    End Sub

    Private Sub HandleMonitoredItemDeleted(sender As Object, e As OpcMonitoredItemEventArgs)
        If e.Item.Attribute <> OpcAttribute.Value Then
            Return
        End If

        SyncLock Me.monitored
            Dim count As Integer = 0

            If Not Me.monitored.TryGetValue(e.Item.NodeId, count) Then
                Return
            End If

            If count > 1 Then
                Me.monitored(e.Item.NodeId) = count - 1
            Else
                Me.monitored.Remove(e.Item.NodeId)
            End If
        End SyncLock
    End Sub
End Class

'DOC
Public Class DeviceNodeManager
    Inherits OpcNodeManager

    Public Sub New()
        MyBase.New("http://example.com/device")

        AddHandler Me.BeforeRead, AddressOf Me.HandleBeforeRead
        AddHandler Me.AfterWrite, AddressOf Me.HandleAfterWrite
    End Sub

    Private Sub HandleBeforeRead(sender As Object, e As OpcNodeAccessEventArgs)
        Dim nodes As OpcDataVariableNode() = e.Tokens _
                .Where(Function(token) token.Attribute = OpcAttribute.Value) _
                .Select(Function(token) token.Node) _
                .OfType(Of OpcDataVariableNode)() _
                .Distinct() _
                .ToArray()

        ' Device stands for the connection to the data source:
        ' one request for all nodes of the client request.
        Dim values As Object() = Device.Read(nodes.Select(Function(node) node.SymbolicName).ToArray())

        For index As Integer = 0 To nodes.Length - 1
            nodes(index).Value = values(index)
        Next
    End Sub

    Private Sub HandleAfterWrite(sender As Object, e As OpcNodeAccessEventArgs)
        Dim written = e.Tokens _
                .Where(Function(token) token.Attribute = OpcAttribute.Value AndAlso token.Result.IsGood) _
                .ToArray()

        ' One request to the device for all values written by the client.
        Device.Write( _
                written.Select(Function(token) token.Node.SymbolicName).ToArray(), _
                written.Select(Function(token) token.Value.Value).ToArray())
    End Sub
End Class

'DOC
Dim temperature = New OpcDataVariableNode(Of Double)("Temperature")
temperature.DisplayName = New OpcText("Temperature", "en-US", "Temperature.DisplayName")
temperature.Description = New OpcText("Temperature of the machine", "en-US", "Temperature.Description")

Using server = New OpcServer("opc.tcp://localhost:4840/", temperature)
    server.Globalization.AddResource("de-DE", "Temperature.DisplayName", "Temperatur")
    server.Globalization.AddResource("de-DE", "Temperature.Description", "Temperatur der Maschine")

    server.Start()
End Using

'DOC
OpcAutomatism.RaiseModelChangeEvents = True

'DOC
Dim machine = New OpcObjectNode(machines, "Machine3")
Dim temperature = New OpcDataVariableNode(Of Double)(machine, "Temperature")

manager.AddNode(server.SystemContext, machine)

'DOC
machines.SuspendModelChangeEvents(recursive:=True)

For Each name As String In {"Machine4", "Machine5", "Machine6"}
    manager.AddNode(server.SystemContext, New OpcObjectNode(machines, name))
Next

machines.ResumeModelChangeEvents(recursive:=True)

'DOC
Dim startJob = New OpcMethodNode( _
        machine, _
        "StartJob", _
        New Func(Of String, Task(Of Integer))(AddressOf StartJobAsync))

'DOC
Private Shared Async Function StartJobAsync(jobName As String) As Task(Of Integer)
    ' Long running work, e.g. waiting for the machine.
    Await Task.Delay(TimeSpan.FromSeconds(5))
    Return 42
End Function

'DOC
Public Class JobNodeManager
    Inherits OpcNodeManager

    Public Sub New()
        MyBase.New("http://example.com/jobs")
    End Sub

    Protected Overrides ReadOnly Property UseAsyncMethodCalls As Boolean
        Get
            Return True
        End Get
    End Property
End Class

'DOC
Using server = New OpcServer("opc.tcp://localhost:4840/")
    Dim namespaces As OpcNamespace() = server.RegisterNamespaces( _
            "http://example.com/machine", _
            "http://example.com/plant")

    Console.WriteLine("Machine: ns={0}", namespaces(0).Index)
    Console.WriteLine("Plant: ns={0}", namespaces(1).Index)

    server.Start()
End Using

'DOC
Dim profile = New OpcDataVariableNode(Of Double())(machine, "Profile", New Double(99) {})
Dim heatMap = New OpcDataVariableNode(Of Double(,))(machine, "HeatMap", New Double(15, 15) {})

'DOC
Dim values As Double(,) = heatMap.Value
values(3, 4) = 72.5

heatMap.Value = values
heatMap.ApplyChanges(server.SystemContext)

'DOC
Dim tags = {
    New With {.Name = "Temperature", .Type = "double"},
    New With {.Name = "IsRunning", .Type = "bool"},
    New With {.Name = "JobName", .Type = "string"}
}

Dim server = New OpcServer("opc.tcp://localhost:4840/", _
        Sub(space)
            Dim machine = New OpcFolderNode("Machine")

            For Each tag In tags
                Select Case tag.Type
                    Case "double"
                        Dim node = New OpcDataVariableNode(Of Double)(machine, tag.Name)
                    Case "bool"
                        Dim node = New OpcDataVariableNode(Of Boolean)(machine, tag.Name)
                    Case Else
                        Dim node = New OpcDataVariableNode(Of String)(machine, tag.Name)
                End Select
            Next

            space.AddNode(machine)
        End Sub)

'DOC
Dim info As OpcServerInfo = server.GetInfo()

Console.WriteLine("{0} {1} {2}", info.CompanyName, info.ProductName, info.ProductVersion)
Console.WriteLine("Product URI: {0}", info.ProductUri)

'DOC
<Assembly: AssemblyCompany("Example Machines Ltd.")>
<Assembly: AssemblyTitle("Example Machine Server")>
<Assembly: AssemblyVersion("1.2.0.0")>

'DOC
Public Class MachineServer
    Inherits OpcServer

    Public Sub New()
        MyBase.New("opc.tcp://localhost:4840/")
    End Sub

    Protected Overrides Function GetInfoCore() As OpcServerInfo
        Return New OpcServerInfo( _
                "Example Machines Ltd.", _
                "Example Machine Server", _
                New Version(1, 2, 0, 0), _
                New DateTime(2026, 1, 15), _
                New Uri("urn:example:machine-server"))
    End Function
End Class
