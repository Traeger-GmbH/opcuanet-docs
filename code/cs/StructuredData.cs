// DOC
client.UseDynamic = true;
client.Connect();
 
dynamic staff = client.ReadNode("ns=2;s=Machine/Operator").Value;
 
// Access the 'Name' and 'ID' field of the data without to declare the data type itself.
// Just use the field names known as they would be defined in a .NET Type.
Console.WriteLine("Name: {0}", staff.Name);
Console.WriteLine("Staff ID: {0}", staff.ID);
 
// Continue accessing subsequently used data types.
Console.WriteLine("Shift: {0}", staff.Shift.Name);
Console.WriteLine("- Time Elapsed: {0}", staff.Shift.Elapsed);
Console.WriteLine("- Jobs Remaining: {0}", staff.Shift.Remaining);
 
// Change Shift
staff.Name = "John";
staff.ID = 4242;
staff.Shift.Name = "Swing Shift";
 
client.WriteNode("ns=2;s=Machine/Operator", staff);

// DOC
client.UseDynamic = true;
client.Connect();
 
OpcDataObject staff = client.ReadNode("ns=2;s=Machine/Operator").As<OpcDataObject>();
 
// Access the 'Name' and 'ID' field of the data without to declare the data type itself.
// Just use the field names known as the 'key' to access the according field value.
Console.WriteLine("Name: {0}", staff["Name"].Value);
Console.WriteLine("Staff ID: {0}", staff["ID"].Value);
 
// Continue accessing subsequently used data types using the OpcDataObject as before.
OpcDataObject shift = (OpcDataObject)staff["Shift"].Value;
 
Console.WriteLine("Shift: {0}", shift["Name"].Value);
Console.WriteLine("- Time Elapsed: {0}", shift["Elapsed"].Value);
Console.WriteLine("- Jobs Remaining: {0}", shift["Remaining"].Value);
 
// Change Shift
staff["Name"].Value = "John";
staff["ID"].Value = 4242;
shift["Name"].Value = "Swing Shift";
 
client.WriteNode("ns=2;s=Machine/Operator", staff);

// DOC
using (OpcClient client = new OpcClient("opc.tcp://localhost:4840")) {
    client.NodeSet = OpcNodeSet.Load(@"..\Resources\MyServersNodeSet.xml");
    client.UseDynamic = true;
 
    client.Connect();
    dynamic staff = client.ReadNode("ns=2;s=Machine/Operator").Value;
 
    Console.WriteLine("Name: {0}", staff.Name);
    Console.WriteLine("Staff ID: {0}", staff.ID);
}

// DOC
[OpcDataType("ns=2;s=StaffType")]
[OpcDataTypeEncoding("ns=2;s=StaffType-Binary")]
public class Staff
{
    public string Name { get; set; }
    public int ID { get; set; }
    public ShiftInfo Shift { get; set; }
}
 
[OpcDataType("ns=2;s=ShiftInfoType")]
[OpcDataTypeEncoding("ns=2;s=ShiftInfoType-Binary")]
public class ShiftInfo
{
    public string Name { get; set; }
    public DateTime Elapsed { get; set; }
    public byte Remaining { get; set; }
}

// DOC
client.Connect();
Staff staff = client.ReadNode("ns=2;s=Machine/Operator").As<Staff>();
 
// Access the 'Name' and 'ID' field of the data with the declared the data type.
Console.WriteLine("Name: {0}", staff.Name);
Console.WriteLine("Staff ID: {0}", staff.ID);
 
// Continue accessing subsequently used data types.
Console.WriteLine("Shift: {0}", staff.Shift.Name);
Console.WriteLine("- Time Elapsed: {0}", staff.Shift.Elapsed);
Console.WriteLine("- Jobs Remaining: {0}", staff.Shift.Remaining);
 
// Change Shift
staff.Name = "John";
staff.ID = 4242;
staff.Shift.Name = "Swing Shift";
 
client.WriteNode("ns=2;s=Machine/Operator", staff);

// DOC
[OpcDataType("<NodeId of DataType Node>")]
[OpcDataTypeEncoding(
        "<NodeId of Binary Encoding Node>",
        NamespaceUri = "<NamespaceUri.Value of binary Dictionary-Node>")]
internal struct MyDataType
{
    public short FieldA;
    public int FieldB;
    public string FieldC;
    ...
}

// DOC
OpcNodeInfo node = client.BrowseNode("ns=2;s=Machine/Operator");
 
if (node is OpcVariableNodeInfo variableNode) {
    OpcNodeId dataTypeId = variableNode.DataTypeId;
    OpcDataTypeInfo dataType = client.GetDataTypeSystem().GetType(dataTypeId);
 
    Console.WriteLine(dataType.TypeId);
    Console.WriteLine(dataType.Encoding);
 
    Console.WriteLine(dataType.Name);
 
    foreach (OpcDataFieldInfo field in dataType.GetFields())
        Console.WriteLine(".{0} : {1}", field.Name, field.FieldType);
 
    Console.WriteLine();        
    Console.WriteLine("Data Type Attributes:");
    Console.WriteLine(
            "\t[OpcDataType(\"{0}\")]",
            dataType.TypeId.ToString(OpcNodeIdFormat.Foundation));
    Console.WriteLine(
            "\t[OpcDataTypeEncoding(\"{0}\", NamespaceUri = \"{1}\")]",
            dataType.Encoding.Id.ToString(OpcNodeIdFormat.Foundation),
            dataType.Encoding.Namespace.Value);
}



// DOC
[OpcDataType("<NodeId of DataType Node>")]
[OpcDataTypeEncoding(
        "<NodeId of Binary Encoding Node>",
        NamespaceUri = "<NamespaceUri.Value of binary Dictionary-Node>")]
[OpcDataTypeEncodingMask(OpcEncodingMaskKind.Auto)]
internal struct MyDataTypeWithOptionalFields
{
    public short FieldA;
    public int FieldB;
    public string FieldC;

    // Nullables are treat as optional fields by default.
    // Existence-Indicator-Bit is Bit0 in the encoding mask.
    public uint? OptionalField1;

    // Existence-Indicator-Bit is Bit1 (the next unused bit) in the encoding mask.
    [OpcDataTypeMemberSwitch]
    public int OptionalField2;

    // Existence-Indicator-Bit is Bit3 (bit 2 is unused) in the encoding mask.
    [OpcDataTypeMemberSwitch(bit: 3)]
    public byte OptionalField3;

    public bool FieldD;

    // 'OptionalField3' exists only if the value of 'FieldD' is equals 'true'.
    [OpcDataTypeMemberSwitch("FieldD")]
    public string OptionalField3;

    public int FieldE;

    // 'OptionalField4' exists only if the value of 'FieldE' is greater than '42'.
    [OpcDataTypeMemberSwitch("FieldE", value: 42, operand: OpcMemberSwitchOperator.GreaterThan)]
    public string OptionalField4;
}
