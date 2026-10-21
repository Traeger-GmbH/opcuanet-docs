// DOC
OpcValue value = client.ReadNode("ns=2;s=Machine/Speed");
 
if (value.Status.IsGood)
    Console.WriteLine("Speed: {0}", value.Value);
else if (value.Status.IsUncertain)
    Console.WriteLine("Speed (uncertain): {0}", value.Value);
else
    Console.WriteLine("Read failed: {0} ({1})", value.Status.Code, value.Status.Description);


// DOC
OpcStatus result = client.WriteNode("ns=2;s=Machine/Setpoint", value: 42.0);
 
if (result.Code == OpcStatusCode.BadUserAccessDenied)
    Console.WriteLine("The user is not allowed to write the setpoint.");
else if (result.IsBad)
    Console.WriteLine("Write failed: {0}", result.Description);


// DOC
try {
    object[] outputs = client.CallMethod(
            "ns=2;s=Machine",
            "ns=2;s=Machine/StartJob",
            "Job 1");
}
catch (OpcException ex) {
    Console.WriteLine("Call failed: {0} ({1})", ex.Code, ex.Message);
}

// DOC
try {
    client.Connect();
}
catch (OpcException ex) {
    if (ex.Code == OpcStatusCode.BadSecurityChecksFailed)
        Console.WriteLine("No endpoint matches the security settings of the client.");
    else
        Console.WriteLine("Connect failed: {0}", ex.Code);
}
