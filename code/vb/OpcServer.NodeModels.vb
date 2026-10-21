'DOC
Dim nodeType As OpcNodeTypeInfo = server.GetNodeType(OpcMachineryNodes.MachineIdentificationType)
Dim model As OpcNodeModel = OpcNodeModel.Of(nodeType)

For Each child As OpcNodeModel In model.Children()
    Console.WriteLine(child)
Next

For Each optionalChild As OpcNodeModel In model.ChildrenWith(OpcModellingRule.Optional)
    Console.WriteLine("Optional: {0}", optionalChild.Name)
Next

'DOC
Dim model As OpcNodeModel = OpcNodeModel _
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType)) _
        .OmitOptionals() _
        .CreateIf(Function(name) name.Value = "YearOfConstruction" OrElse name.Value = "Location")

Dim machine = New OpcObjectNode(machinery.Machines, "CNC-042")
Dim identification = New OpcMachineIdentificationNode(model, machine, "Identification")

identification.Manufacturer.Value = "Example Machines Ltd."
identification.SerialNumber.Value = "SN-0042"
identification.ProductInstanceUri.Value = "urn:example:cnc-0042"
identification.YearOfConstruction.Value = 2026

machinery.AddNode(server.SystemContext, machine)

'DOC
Dim model As OpcNodeModel = OpcNodeModel _
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType)) _
        .OmitOptionals(recursive:=True)

'DOC
Dim model As OpcNodeModel = OpcNodeModel _
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType)) _
        .OmitOptionals() _
        .OmitByDefault()

'DOC
Dim template As IOpcNodeInfo = nodeSet.GetNode(templateId)

Dim model As OpcNodeModel = OpcNodeModel _
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType)) _
        .OmitOptionals() _
        .CreateLike(template)

'DOC
' MachineIdentificationType defines its own property "Model" (product model).
' The node model is available via the base class OpcNode.
Dim nodeModel As OpcNodeModel = CType(identification, OpcNode).Model

Console.WriteLine(nodeModel.Directive)
Console.WriteLine(If(identification.Location Is Nothing, "Location omitted", "Location created"))
