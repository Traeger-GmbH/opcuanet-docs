// DOC
public enum MachineState : uint
{
    Stopped = 0,
    Starting = 1,
    Running = 2,
    Stopping = 3
}

// DOC
[OpcStateMachineStates(typeof(MachineState))]
[OpcStateMachineTransition(MachineState.Stopped, MachineState.Starting)]
[OpcStateMachineTransition(MachineState.Starting, MachineState.Running)]
[OpcStateMachineTransition(MachineState.Running, MachineState.Stopping)]
[OpcStateMachineTransition(MachineState.Stopping, MachineState.Stopped)]
[OpcStateMachineInitialState(MachineState.Stopped)]
public class MachineStateMachineNode : OpcFiniteStateMachineNode<MachineState>
{
    public MachineStateMachineNode(IOpcNode parent, OpcName name)
        : base(parent, name)
    {
    }
}

// DOC
// Transitions from each state to the next one and from the last state to the first one.
[OpcStateMachineStates(typeof(MachineState))]
[OpcStateMachineTransitions(typeof(MachineState), IsLoop = true)]
public class MachineCycleNode : OpcFiniteStateMachineNode<MachineState>
{
    public MachineCycleNode(IOpcNode parent, OpcName name)
        : base(parent, name)
    {
    }
}

// DOC
// MyNodeManager.CreateNodes(...)
var machineNode = new OpcObjectNode(new OpcName("Machine", this.DefaultNamespaceIndex));
var machineStateNode = new MachineStateMachineNode(machineNode, "State");

// DOC
if (machineStateNode.CanChangeStateTo(server.SystemContext, MachineState.Starting))
    machineStateNode.ChangeStateTo(server.SystemContext, MachineState.Starting);

// Throws an InvalidOperationException, because there is no transition
// from the state Starting to the state Stopped.
machineStateNode.ChangeStateTo(server.SystemContext, MachineState.Stopped);
