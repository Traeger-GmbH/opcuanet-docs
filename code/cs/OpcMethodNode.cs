// DOC
// The result array contains the values of the OUT arguments offered by the method.
object[] result = client.CallMethod(
        "ns=2;s=Machine",                  /* NodeId of Owner Node */
        "ns=2;s=Machine/StartMachine"      /* NodeId of Method Node*/);

// DOC
// The result array contains the values of the OUT arguments offered by the method.
object[] result = client.CallMethod(
        "ns=2;s=Machine",                  /* NodeId of Owner Node */
        "ns=2;s=Machine/StopMachine",      /* NodeId of Method Node */
        "Job Change",                      /* Parameter 1: 'reason' */
        10023,                             /* Parameter 2: 'reasonCode' */
        DateTime.Now                       /* Parameter 3: 'scheduleDate' */);

// DOC
OpcCallMethod[] commands = new OpcCallMethod[] {
    new OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/StopMachine"),
    new OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/ScheduleJob"),
    new OpcCallMethod("ns=2;s=Machine", "ns=2;s=Machine/StartMachine")
};
 
// The result array contains the values of the OUT arguments offered by the methods.
object[][] results = client.CallMethods(commands);

// DOC
OpcCallMethod[] commands = new OpcCallMethod[] {
    new OpcCallMethod(
            "ns=2;s=Machine",              /* NodeId of Owner Node */ 
            "ns=2;s=Machine/StopMachine",  /* NodeId of Method Node */
            "Job Change",                  /* Parameter 1: 'reason' */
            10023,                         /* Parameter 2: 'reasonCode' */
            DateTime.Now                   /* Parameter 3: 'scheduleDate' */),
    new OpcCallMethod(
            "ns=2;s=Machine",              /* NodeId of Owner Node */
            "ns=2;s=Machine/ScheduleJob",  /* NodeId of Method Node */
            "MAN_F01_78910"                /* Parameter 1: 'jobSerial' */),
    new OpcCallMethod(
            "ns=2;s=Machine",              /* NodeId of Owner Node */
            "ns=2;s=Machine/StartMachine", /* NodeId of Method Node */
            10021                          /* Parameter 1: 'reasonCode' */)
};
 
// The result array contains the values of the OUT arguments offered by the methods.
object[][] results = client.CallMethods(commands);
