'DOC
Using server = New OpcMachineryServer("opc.tcp://localhost:4840/")
    server.Start()

    Dim machinery As OpcMachineryNodeManager = server.NodeManagers _
            .OfType(Of OpcMachineryNodeManager)() _
            .First()

    Dim machine = New OpcObjectNode(machinery.Machines, "CNC-042")
    Dim identification = New OpcMachineIdentificationNode(machine, "Identification")

    identification.Manufacturer.Value = "Example Machines Ltd."
    identification.SerialNumber.Value = "SN-0042"
    identification.ProductInstanceUri.Value = "urn:example:cnc-0042"

    machinery.AddNode(server.SystemContext, machine)
End Using

'DOC
Public Class HandmadeMachineManager
    Inherits OpcNodeManager

    Private Shared ReadOnly HasInterface As OpcNodeId = OpcNodeId.Parse("i=17603")

    Public Sub New()
        MyBase.New("http://example.com/machines/")
    End Sub

    Public Shared Function CreateServer() As OpcServer
        Dim nodeSets = OpcNodeSetManager.Create( _
                OpcNodeSet.Load("NodeSets\Opc.Ua.Di.NodeSet2.xml"), _
                OpcNodeSet.Load("NodeSets\Opc.Ua.IA.NodeSet2.xml"), _
                OpcNodeSet.Load("NodeSets\Opc.Ua.Machinery.NodeSet2.xml"))

        Return New OpcServer("opc.tcp://localhost:4840/", nodeSets, New HandmadeMachineManager())
    End Function

    Protected Overrides Iterator Function CreateNodes( _
            ByVal references As OpcNodeReferenceCollection) As IEnumerable(Of IOpcNode)
        Dim di = OpcNamespace.Create("http://opcfoundation.org/UA/DI/")
        Dim machinery = OpcNamespace.Create("http://opcfoundation.org/UA/Machinery/")
        di.Resolve(Me)
        machinery.Resolve(Me)

        'Machinery: Machines folder (i=1001).
        Dim machine = New OpcObjectNode("CNC-042")

        references.Add( _
                machine, _
                machinery.GetId(1001), _
                direction:=OpcReferenceDirection.ChildToParent, _
                referenceType:=OpcReferenceType.Organizes)

        'Machinery: MachineIdentificationType (i=1012).
        Dim identification = New OpcObjectNode(machine, machinery.GetName("Identification"))
        identification.TypeDefinitionId = machinery.GetId(1012)

        'Mandatory properties: names, namespaces and data types from the specification.
        Dim manufacturer = New OpcTextPropertyNode( _
                identification, _
                di.GetName("Manufacturer"), _
                value:=New OpcText("Example Machines Ltd."))

        Dim serialNumber = New OpcPropertyNode(Of String)( _
                identification, _
                di.GetName("SerialNumber"), _
                id:=OpcNodeId.Null, _
                value:="SN-0042")

        Dim instanceUri = New OpcPropertyNode(Of String)( _
                identification, _
                di.GetName("ProductInstanceUri"), _
                id:=OpcNodeId.Null, _
                value:="urn:example:cnc-0042")

        'Interfaces of the type: IMachineVendorNameplateType (i=1010),
        'IMachineTagNameplateType (i=1011).
        references.Add( _
                identification, _
                machinery.GetId(1010), _
                direction:=OpcReferenceDirection.ParentToChild, _
                referenceTypeId:=HasInterface)

        references.Add( _
                identification, _
                machinery.GetId(1011), _
                direction:=OpcReferenceDirection.ParentToChild, _
                referenceTypeId:=HasInterface)

        Yield machine
    End Function
End Class


'DOC
nodeSets.ImplementNodeCallback = Sub(context, node)
    Dim method = TryCast(node, OpcMethodNode)

    If method IsNot Nothing AndAlso method.Name.Value = "TransferToDevice" Then
        method.Command = New TransferToDeviceCommand()
    End If
End Sub
...
Public NotInheritable Class TransferToDeviceCommand
    Implements IOpcMethodCommand

    Public Function CanExecute(ByVal context As OpcContext) As Boolean _
            Implements IOpcMethodCommand.CanExecute
        Return True
    End Function

    Public Function CanUserExecute(ByVal context As OpcContext) As Boolean _
            Implements IOpcMethodCommand.CanUserExecute
        Return True
    End Function

    Public Sub Execute( _
            ByVal context As OpcMethodContext, _
            ByVal inputArguments As IList(Of Object), _
            ByVal outputArguments As IList(Of Object)) _
            Implements IOpcMethodCommand.Execute
        'Order and types of the arguments as defined by the specification.
        outputArguments(0) = 1 'TransferID
        outputArguments(1) = 0 'InitTransferStatus
    End Sub
End Class

'DOC
Public Class DeviceTransferServices
    Inherits OpcTransferServicesNode

    Public Sub New(ByVal parent As IOpcNode)
        MyBase.New(parent, "TransferServices")
    End Sub

    Public Overrides Sub TransferToDevice( _
            ByVal methodContext As OpcMethodContext, _
            ByRef transferID As Integer, _
            ByRef initTransferStatus As Integer)
        transferID = 1
        initTransferStatus = 0
    End Sub
End Class


'DOC
Using client = New OpcMachineryClient("opc.tcp://localhost:4840/")
    client.Connect()

    Dim machines As OpcNodeInfo = client.BrowseNode(OpcMachineryNodes.MachinesNode)

    For Each machine As OpcNodeInfo In machines.Children()
        Console.WriteLine(machine.DisplayName.Value)
    Next
End Using
