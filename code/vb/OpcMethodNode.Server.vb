'DOC
Dim startMethodNode = New OpcMethodNode( _
        machineNode, _
        "StartMachine", _
        New Action(AddressOf Me.StartMachine))
...
Private Sub StartMachine()
    'Your code to execute.
End Sub


'DOC
Dim startMethodNode = New OpcMethodNode( _
        machineNode, _
        "StartMachine", _
        New Action(Of Integer)(AddressOf Me.StartMachine))
...
Private Sub StartMachine(ByVal reasonNumber As Integer)
    'Your code to execute.
End Sub


'DOC
Dim startMethodNode = New OpcMethodNode( _
        machineNode, _
        "StartMachine", _
        New Func(Of Integer)(AddressOf Me.StartMachine))
...
Private Function StartMachine() As Integer
    'Your code to execute.
    Return statusCode;
End Function


'DOC
Dim startMethodNode = New OpcMethodNode( _
        machineNode, _
        "StartMachine", _
        New Func(Of Integer, String, Integer)(AddressOf Me.StartMachine))
...
Private Function StartMachine(ByVal reasonNumber As Integer, ByVal operatorName As String) As Integer
    'Your code to execute.
    Return statusCode;
End Function


'DOC
Dim startMethodNode = New OpcMethodNode( _
        machineNode, _
        "StartMachine", _
        New Func(Of OpcMethodNodeContext, Integer, Integer)(AddressOf Me.StartMachine))
...
Private Function StartMachine(ByVal context As OpcMethodNodeContext, ByVal reasonNumber As Integer) As Integer
    'Your code to execute.

    Me.machineStateVariable.Value = "Started"
    Me.machineStateVariable.ApplyChanges(context)

    Return statusCode
End Function


'DOC
<OpcArgument("Result", Description:="The result code of the machine driver.")>
Private Function StartMachine(
        <OpcArgument("ReasonNumber", Description:="0: Maintenance, 1: Manufacturing, 2: Service")> _
        ByVal reasonNumber As Integer,
        <OpcArgument("OperatorName", Description:="Optional. Name of the operator of the current shift.")> _
        ByVal operatorName As String) As Integer
    'Your code to execute.
    Return 10
End Function

