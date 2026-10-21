// DOC
OpcNodeTypeInfo nodeType = server.GetNodeType(OpcMachineryNodes.MachineIdentificationType);
OpcNodeModel model = OpcNodeModel.Of(nodeType);

foreach (OpcNodeModel child in model.Children())
    Console.WriteLine(child);

foreach (OpcNodeModel optionalChild in model.ChildrenWith(OpcModellingRule.Optional))
    Console.WriteLine("Optional: {0}", optionalChild.Name);

// DOC
OpcNodeModel model = OpcNodeModel
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType))
        .OmitOptionals()
        .CreateIf(name => name.Value == "YearOfConstruction" || name.Value == "Location");

var machine = new OpcObjectNode(machinery.Machines, "CNC-042");
var identification = new OpcMachineIdentificationNode(model, machine, "Identification");

identification.Manufacturer.Value = "Example Machines Ltd.";
identification.SerialNumber.Value = "SN-0042";
identification.ProductInstanceUri.Value = "urn:example:cnc-0042";
identification.YearOfConstruction.Value = 2026;

machinery.AddNode(server.SystemContext, machine);

// DOC
OpcNodeModel model = OpcNodeModel
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType))
        .OmitOptionals(recursive: true);

// DOC
OpcNodeModel model = OpcNodeModel
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType))
        .OmitOptionals()
        .OmitByDefault();

// DOC
IOpcNodeInfo template = nodeSet.GetNode(templateId);

OpcNodeModel model = OpcNodeModel
        .Of(server.GetNodeType(OpcMachineryNodes.MachineIdentificationType))
        .OmitOptionals()
        .CreateLike(template);

// DOC
// MachineIdentificationType defines its own property "Model" (product model).
// The node model is available via the base class OpcNode.
OpcNodeModel nodeModel = ((OpcNode)identification).Model;

Console.WriteLine(nodeModel.Directive);
Console.WriteLine(identification.Location == null ? "Location omitted" : "Location created");
