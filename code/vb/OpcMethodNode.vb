'DOC
'The result array contains the values of the OUT arguments offered by the method.
Dim result As Object() = client.CallMethod( _
        "ns=2;s=Machine", _                     'NodeId of Owner Node
        "ns=2;s=Machine/StartMachine")          'NodeId of Method Node

'DOC
'The result array contains the values of the OUT arguments offered by the method.
Dim result As Object() = client.CallMethod( _
        "ns=2;s=Machine", _                     'NodeId of Owner Node
        "ns=2;s=Machine/StopMachine", _         'NodeId of Method Node
        "Job Change", _                         'Parameter 1: 'reason'
        10023, _                                'Parameter 2: 'reasonCode'
        DateTime.Now)                           'Parameter 3: 'scheduleDate'

'DOC
Dim commands As OpcCallMethod() = New OpcCallMethod() { _
    New OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/StopMachine"), _
    New OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/ScheduleJob"), _
    New OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/StartMachine") _
}

'The result array contains the values of the OUT arguments offered by the methods.
Dim results As Object()() = client.CallMethods(commands)

'DOC
Dim commands As OpcCallMethod() = New OpcCallMethod() { _
    New OpcCallMethod( _
            "ns=2;s=Machine", _                 'NodeId of Owner Node
            "ns=2;s=Machine/StopMachine", _     'NodeId of Method Node
            "Job Change", _                     'Parameter 1: 'reason'
            10023, _                            'Parameter 2: 'reasonCode'
            DateTime.Now), _                    'Parameter 3: 'scheduleDate'
    New OpcCallMethod( _
            "ns=2;s=Machine", _                 'NodeId of Owner Node
            "ns=2;s=Machine/ScheduleJob", _     'NodeId of Method Node
            "MAN_F01_78910"), _                 'Parameter 1: 'jobSerial'
    New OpcCallMethod( _
            "ns=2;s=Machine", _                 'NodeId of Owner Node
            "ns=2;s=Machine/StartMachine", _    'NodeId of Method Node
            10021) _                            'Parameter 1: 'reasonCode'
}

'The result array contains the values of the OUT arguments offered by the methods.
Dim results As Object()() = client.CallMethods(commands)

'DOC
'Call a method without IN and OUT arguments.
client.CallAction("ns=2;s=Machine", "ns=2;s=Machine/StartMachine")

'Call a method with two IN arguments and return the value of its first OUT argument.
Dim jobId As Integer = client.CallFunc(Of String, Integer, Integer)( _
        "ns=2;s=Machine", _
        "ns=2;s=Machine/StartJob", _
        "JOB001", _
        10023)
