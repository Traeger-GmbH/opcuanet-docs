'DOC
client.UseDynamic = True
client.Connect()

Dim staff = client.ReadNode("ns=2;s=Machine/Operator").Value

'Access the 'Name' and 'ID' field of the data without to declare the data type itself.
'Just use the field names known as they would be defined in a .NET Type.
Console.WriteLine("Name: {0}", staff.Name)
Console.WriteLine("Staff ID: {0}", staff.ID)

'Continue accessing subsequently used data types.
Console.WriteLine("Shift: {0}", staff.Shift.Name)
Console.WriteLine("- Time Elapsed: {0}", staff.Shift.Elapsed)
Console.WriteLine("- Jobs Remaining: {0}", staff.Shift.Remaining)

'Change Shift
staff.Name = "John"
staff.ID = 4242
staff.Shift.Name = "Swing Shift"

client.WriteNode("ns=2;s=Machine/Operator", staff)

'DOC
client.UseDynamic = True
client.Connect()

Dim staff As OpcDataObject = client.ReadNode("ns=2;s=Machine/Operator").[As](Of OpcDataObject)()

'Access the 'Name' and 'ID' field of the data without to declare the data type itself.
'Just use the field names known as the 'key' to access the according field value.
Console.WriteLine("Name: {0}", staff("Name").Value)
Console.WriteLine("Staff ID: {0}", staff("ID").Value)

'Continue accessing subsequently used data types using the OpcDataObject as before.
Dim shift As OpcDataObject = CType(staff("Shift").Value, OpcDataObject)

Console.WriteLine("Shift: {0}", shift("Name").Value)
Console.WriteLine("- Time Elapsed: {0}", shift("Elapsed").Value)
Console.WriteLine("- Jobs Remaining: {0}", shift("Remaining").Value)

'Change Shift
staff("Name").Value = "John"
staff("ID").Value = 4242
shift("Name").Value = "Swing Shift"

client.WriteNode("ns=2;s=Machine/Operator", staff)

'DOC
Using Dim client As New OpcClient("opc.tcp://localhost:4840")
    client.NodeSet = OpcNodeSet.Load("..\Resources\MyServersNodeSet.xml")
    client.UseDynamic = True

    client.Connect()
    Dim staff = client.ReadNode("ns=2;s=Machine/Operator").Value

    Console.WriteLine("Name: {0}", staff.Name)
    Console.WriteLine("Staff ID: {0}", staff.ID)
End Using

'DOC
<OpcDataType("ns=2;s=StaffType")>
<OpcDataTypeEncoding("ns=2;s=StaffType-Binary")>
Public Class Staff
    Public Property Name As String
    Public Property ID As Integer
    Public Property Shift As ShiftInfo
End Class


<OpcDataType("ns=2;s=ShiftInfoType")>
<OpcDataTypeEncoding("ns=2;s=ShiftInfoType-Binary")>
Public Class ShiftInfo
    Public Property Name As String
    Public Property Elapsed As DateTime
    Public Property Remaining As Byte
End Class


'DOC
client.Connect()
Dim staff As Staff = client.ReadNode("ns=2;s=Machine/Operator").[As](Of Staff)()

'Access the 'Name' and 'ID' field of the data with the declared the data type.
Console.WriteLine("Name: {0}", staff.Name)
Console.WriteLine("Staff ID: {0}", staff.ID)

'Continue accessing subsequently used data types.
Console.WriteLine("Shift: {0}", staff.Shift.Name)
Console.WriteLine("- Time Elapsed: {0}", staff.Shift.Elapsed)
Console.WriteLine("- Jobs Remaining: {0}", staff.Shift.Remaining)

'Change Shift
staff.Name = "John"
staff.ID = 4242
staff.Shift.Name = "Swing Shift"

client.WriteNode("ns=2;s=Machine/Operator", staff)

'DOC
<OpcDataType("<NodeId of DataType Node>")>
<OpcDataTypeEncoding( _
        "<NodeId of Binary Encoding Node>", _
        NamespaceUri:="<NamespaceUri.Value of binary Dictionary-Node>")>
Friend Structure MyDataType
    Public FieldA As Short
    Public FieldB As Integer
    Public FieldC As String
    ...
End Structure


'DOC
Dim node As OpcNodeInfo = client.BrowseNode("ns=2;s=Machine/Operator")
Dim variableNode As OpcVariableNodeInfo = TryCast(node, OpcVariableNodeInfo)

If variableNode IsNot Nothing Then
    Dim dataTypeId As OpcNodeId = variableNode.DataTypeId
    Dim dataType As OpcDataTypeInfo = client.GetDataTypeSystem().[GetType](dataTypeId)

    Console.WriteLine(dataType.TypeId)
    Console.WriteLine(dataType.Encoding)

    Console.WriteLine(dataType.Name)

    For Each field As OpcDataFieldInfo In dataType.GetFields()
        Console.WriteLine(".{0} : {1}", field.Name, field.FieldType)
    Next

    Console.WriteLine()
    Console.WriteLine("Data Type Attributes:")
    Console.WriteLine( _
            vbTab & "[OpcDataType(""{0}"")]", _
            dataType.TypeId.ToString(OpcNodeIdFormat.Foundation))
    Console.WriteLine( _
            vbTab & "[OpcDataTypeEncoding(""{0}"", NamespaceUri = ""{1}"")]", _
            dataType.Encoding.Id.ToString(OpcNodeIdFormat.Foundation), _
            dataType.Encoding.[Namespace].Value)
End If

'DOC
<OpcDataType("<NodeId of DataType Node>")>
<OpcDataTypeEncoding( _
        "<NodeId of Binary Encoding Node>", _
        NamespaceUri:="<NamespaceUri.Value of binary Dictionary-Node>")>
<OpcDataTypeEncodingMask(OpcEncodingMaskKind.Auto)>
Friend Structure MyDataTypeWithOptionalFields
    Public FieldA As Short
    Public FieldB As Integer
    Public FieldC As String

    'Nullables are treat as optional fields by default.
    'Existence-Indicator-Bit is Bit0 in the encoding mask.
    Public OptionalField1 As UInteger?

    'Existence-Indicator-Bit is Bit1 (the next unused bit) in the encoding mask.
    <OpcDataTypeMemberSwitch>
    Public OptionalField2 As Integer

    'Existence-Indicator-Bit is Bit3 (bit 2 is unused) in the encoding mask.
    <OpcDataTypeMemberSwitch(bit:=3)>
    Public OptionalField3 As Byte

    Public FieldD As Boolean

    ''OptionalField3' exists only if the value of 'FieldD' is equals 'true'.
    <OpcDataTypeMemberSwitch("FieldD")>
    Public OptionalField3 As String

    Public FieldE As Integer

    ''OptionalField4' exists only if the value of 'FieldE' is greater than '42'.
    <OpcDataTypeMemberSwitch("FieldE", value:=42, operand:=OpcMemberSwitchOperator.GreaterThan)>
    Public OptionalField4 As String
End Structure

