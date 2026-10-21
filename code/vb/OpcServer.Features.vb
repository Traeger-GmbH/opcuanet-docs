'DOC
Dim endTime As DateTime = DateTime.UtcNow
Dim startTime As DateTime = endTime.AddHours(-8)

Dim averagePerHour = client.ReadNodeHistoryProcessed( _
        startTime, _
        endTime, _
        OpcAggregateType.Average, _
        aggregateInterval:=3600000, _
        nodeId:="ns=2;s=Machine/Temperature")

Dim atNoon As OpcHistoryValue = client.ReadNodeHistoryAtTime( _
        time:=DateTime.UtcNow.Date.AddHours(12), _
        nodeId:="ns=2;s=Machine/Temperature")

'DOC
Public Class MachineNodeManager
    Inherits OpcNodeManager

    Private ReadOnly monitored As New Dictionary(Of OpcNodeId, Integer)()

    Public Sub New()
        MyBase.New("http://example.com/machine")

        AddHandler Me.MonitoredItemCreated, AddressOf Me.HandleMonitoredItemCreated
        AddHandler Me.MonitoredItemDeleted, AddressOf Me.HandleMonitoredItemDeleted
    End Sub

    Public Function IsMonitored(ByVal nodeId As OpcNodeId) As Boolean
        SyncLock Me.monitored
            Return Me.monitored.ContainsKey(nodeId)
        End SyncLock
    End Function

    Private Sub HandleMonitoredItemCreated( _
            ByVal sender As Object, _
            ByVal e As OpcMonitoredItemEventArgs)
        If e.Item.Attribute <> OpcAttribute.Value Then
            Return
        End If

        SyncLock Me.monitored
            Dim count As Integer
            Me.monitored.TryGetValue(e.Item.NodeId, count)
            Me.monitored(e.Item.NodeId) = count + 1
        End SyncLock
    End Sub

    Private Sub HandleMonitoredItemDeleted( _
            ByVal sender As Object, _
            ByVal e As OpcMonitoredItemEventArgs)
        If e.Item.Attribute <> OpcAttribute.Value Then
            Return
        End If

        SyncLock Me.monitored
            Dim count As Integer

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

    Private Sub HandleBeforeRead(ByVal sender As Object, ByVal e As OpcNodeAccessEventArgs)
        Dim nodes = New List(Of OpcDataVariableNode)()

        For Each token In e.Tokens
            Dim node = TryCast(token.Node, OpcDataVariableNode)

            If token.Attribute = OpcAttribute.Value AndAlso node IsNot Nothing _
                    AndAlso Not nodes.Contains(node) Then
                nodes.Add(node)
            End If
        Next

        Dim names = New String(nodes.Count - 1) {}

        For index As Integer = 0 To nodes.Count - 1
            names(index) = nodes(index).SymbolicName
        Next

        'Device stands for the connection to the data source:
        'one request for all nodes of the client request.
        Dim values As Object() = Device.Read(names)

        For index As Integer = 0 To nodes.Count - 1
            nodes(index).Value = values(index)
        Next
    End Sub

    Private Sub HandleAfterWrite(ByVal sender As Object, ByVal e As OpcNodeAccessEventArgs)
        Dim names = New List(Of String)()
        Dim values = New List(Of Object)()

        For Each token In e.Tokens
            If token.Attribute = OpcAttribute.Value AndAlso token.Result.IsGood Then
                names.Add(token.Node.SymbolicName)
                values.Add(token.Value.Value)
            End If
        Next

        'One request to the device for all values written by the client.
        Device.Write(names.ToArray(), values.ToArray())
    End Sub
End Class

'DOC
Dim temperature = New OpcDataVariableNode(Of Double)("Temperature")

temperature.DisplayName = New OpcText( _
        "Temperature", _
        cultureName:="en-US", _
        key:="Temperature.DisplayName")

temperature.Description = New OpcText( _
        "Temperature of the machine", _
        cultureName:="en-US", _
        key:="Temperature.Description")

Using server = New OpcServer("opc.tcp://localhost:4840/", temperature)
    server.Globalization.AddResource( _
            cultureName:="de-DE", _
            key:="Temperature.DisplayName", _
            value:="Temperatur")

    server.Globalization.AddResource( _
            cultureName:="de-DE", _
            key:="Temperature.Description", _
            value:="Temperatur der Maschine")

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

For Each name In New String() {"Machine4", "Machine5", "Machine6"}
    manager.AddNode(server.SystemContext, New OpcObjectNode(machines, name))
Next

machines.ResumeModelChangeEvents(recursive:=True)

'DOC
Dim startJobNode = New OpcMethodNode( _
        machine, _
        "StartJob", _
        New Func(Of String, Task(Of Integer))(AddressOf StartJobAsync))

'DOC
Private Shared Async Function StartJobAsync(ByVal jobName As String) As Task(Of Integer)
    'Long running work, e.g. waiting for the machine.
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
Dim profile = New OpcDataVariableNode(Of Double())( _
        machine, _
        "Profile", _
        value:=New Double(99) {})

Dim heatMap = New OpcDataVariableNode(Of Double(,))( _
        machine, _
        "HeatMap", _
        value:=New Double(15, 15) {})

'DOC
Dim values As Double(,) = heatMap.Value
values(3, 4) = 72.5

heatMap.Value = values
heatMap.ApplyChanges(server.SystemContext)

'DOC
'Name and data type of each variable, e.g. read from a configuration file.
Dim tags = New Dictionary(Of String, String) From { _
    {"Temperature", "double"}, _
    {"IsRunning", "bool"}, _
    {"JobName", "string"} _
}

Dim server = New OpcServer("opc.tcp://localhost:4840/", Sub(addressSpace)
    Dim machine = New OpcFolderNode("Machine")

    For Each tag In tags
        Select Case tag.Value
            Case "double"
                Dim node = New OpcDataVariableNode(Of Double)(machine, tag.Key)
            Case "bool"
                Dim node = New OpcDataVariableNode(Of Boolean)(machine, tag.Key)
            Case Else
                Dim node = New OpcDataVariableNode(Of String)(machine, tag.Key)
        End Select
    Next

    addressSpace.AddNode(machine)
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
                companyName:="Example Machines Ltd.", _
                productName:="Example Machine Server", _
                productVersion:=New Version(1, 2, 0, 0), _
                buildDate:=New DateTime(2026, 1, 15), _
                productUri:=New Uri("urn:example:machine-server"))
    End Function
End Class

