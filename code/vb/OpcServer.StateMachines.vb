'DOC
Public Enum MachineState As UInteger
    Stopped = 0
    Starting = 1
    Running = 2
    Stopping = 3
End Enum


'DOC
<OpcStateMachineStates(GetType(MachineState))>
<OpcStateMachineTransition(MachineState.Stopped, MachineState.Starting)>
<OpcStateMachineTransition(MachineState.Starting, MachineState.Running)>
<OpcStateMachineTransition(MachineState.Running, MachineState.Stopping)>
<OpcStateMachineTransition(MachineState.Stopping, MachineState.Stopped)>
<OpcStateMachineInitialState(MachineState.Stopped)>
Public Class MachineStateMachineNode
    Inherits OpcFiniteStateMachineNode(Of MachineState)

    Public Sub New(ByVal parent As IOpcNode, ByVal name As OpcName)
        MyBase.New(parent, name)
    End Sub
End Class

'DOC
'Transitions from each state to the next one and from the last state to the first one.
<OpcStateMachineStates(GetType(MachineState))>
<OpcStateMachineTransitions(GetType(MachineState), IsLoop:=True)>
Public Class MachineCycleNode
    Inherits OpcFiniteStateMachineNode(Of MachineState)

    Public Sub New(ByVal parent As IOpcNode, ByVal name As OpcName)
        MyBase.New(parent, name)
    End Sub
End Class

'DOC
'MyNodeManager.CreateNodes(...)
Dim machineNode = New OpcObjectNode(New OpcName("Machine", Me.DefaultNamespaceIndex))
Dim machineStateNode = New MachineStateMachineNode(machineNode, "State")

'DOC
If machineStateNode.CanChangeStateTo(server.SystemContext, MachineState.Starting) Then
    machineStateNode.ChangeStateTo(server.SystemContext, MachineState.Starting)
End If

'Throws an InvalidOperationException, because there is no transition
'from the state Starting to the state Stopped.
machineStateNode.ChangeStateTo(server.SystemContext, MachineState.Stopped)
