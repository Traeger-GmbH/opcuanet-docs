Private Delegate Sub CalculateDelegate( _
        ByVal a As Integer, _
        ByVal b As Integer, _
        ByRef c As Integer, _
        <Out> ByRef additionResult As Integer, _
        <Out> ByRef differenceResult As Integer)

Private Sub Calculate( _
        <OpcArgument("a", Description:="The left operand.")> _
        ByVal a As Integer, _
        <OpcArgument("b", Description:="The right operand.")> _
        ByVal b As Integer, _
        <OpcArgument("c", Description:="Factor")> _
        ByRef c As Integer, _
        <OpcArgument("add", Description:="The result of addition."), Out> _
        ByRef additionResult As Integer, _
        <OpcArgument("diff", Description:="The result of difference."), Out> _
        ByRef differenceResult As Integer)
    additionResult = (a + b) * c
    differenceResult = (a - b) * c
End Sub

Me.methodNode = New OpcMethodNode( _
        Me.folderNode, _
        NameOf(Calculate), _
        New CalculateDelegate(AddressOf Me.Calculate))
